import React from 'react';
import agent from '../../agent';
import MediaGallery from './MediaGallery';
import { connect } from 'react-redux';
import { PUPPY_PAGE_LOADED, PUPPY_PAGE_UNLOADED } from '../../constants/actionTypes';

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
    // const markup = { __html: marked(this.props.body, { sanitize: true }) };
    const canModify = this.props.currentUser &&
      this.props.currentUser.nameIdentifier === this.props.match.params.id;
    return (
      <div className="article-page">
        <div className="container page">
          <div className="row">
            <div className="col-md-4">
              <img className="card-img-top img-profile" src={this.props.puppy.profile} alt={this.props.puppy.alias} ></img>
            </div>
            <div className="col-md-4">
              <h1>{this.props.puppy.alias}</h1>
              <p>Country: {this.props.puppy.localization.country}</p>
              <p>City: {this.props.puppy.localization.city}</p>
              <p>Street: {this.props.puppy.localization.street}</p>
            </div>
          </div>
        </div>
        <div className="container page">
          <div className="row">
          <MediaGallery
          />
          </div>
        </div>
      </div>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Puppy);
