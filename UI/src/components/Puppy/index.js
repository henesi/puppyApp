import React from 'react';
import agent from '../../agent';
import MediaGallery from './MediaGallery';
import Information from './Information';
import { connect } from 'react-redux';
import { PUPPY_PAGE_LOADED, PUPPY_PAGE_UNLOADED } from '../../constants/actionTypes';
import ModalImage from "react-modal-image";

const PuppyDescriptionInView = props => {
  if (props.description) {
    return (
      <div className="container page">
          <hr></hr>
        <div className="container page">
          <div dangerouslySetInnerHTML={{ __html: props.description }} />
        </div>  
      </div>
    );
  }
  return null;
};

const PuppyLackOfInformationView = props => {
  if (!props.description && props.puppyMedia.length === 0) {
    return (
      <div className="container page">
        <div className="container page">
          <p>Description not yet added. Stay tuned</p>
        </div>  
      </div>
    );
  }
  return null;
};

const PuppyMediaInView = props => {
  if (props.puppyMedia.length > 0) {
    return (
      <div className="container page">
        <hr></hr>
        <div className="container page">
          <div className="row">
          <MediaGallery
          />
          </div>
        </div>
      </div>
    );
  }
  return null;
};

const mapStateToProps = state => ({
  ...state.puppy,
  currentUser: state.common.currentUser
});

const mapDispatchToProps = dispatch => ({
  onLoad: payload =>
    dispatch({ type: PUPPY_PAGE_LOADED, payload }),
  onUnload: () =>
    dispatch({ type: PUPPY_PAGE_UNLOADED })
});

class Puppy extends React.Component {
  componentWillMount() {
    this.props.onLoad(Promise.all([
      agent.Puppies.get(this.props.match.params.id),
      agent.Media.get(this.props.match.params.id)
    ]));
  }

  componentWillUnmount() {
    this.props.onUnload();
  }

  render() {
    if (!this.props.puppy) {
      return null;
    }

    return (
      <div className="article-page">
        <div className="container page">
          <div className="row">
            <div className="col-md-4">
            <ModalImage
              className="card-img-top img-gallery"
              small={this.props.puppy.profile}
              large={this.props.puppy.profile}
              hideDownload={true}
              hideZoom={true}
              alt={this.props.puppy.alias + "_profile"}
            />
            </div>
            <Information
                puppy={this.props.puppy}
                />
            </div>
          </div>
        <PuppyLackOfInformationView puppyMedia={this.props.puppyMedia} description={this.props.puppy.description} />
        <PuppyDescriptionInView description={this.props.puppy.description} /> 
        <PuppyMediaInView puppyMedia={this.props.puppyMedia} /> 
      </div>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Puppy);
