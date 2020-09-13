import React from 'react';
import { connect } from 'react-redux';
import { CHANGE_TAB } from '../../constants/actionTypes';
import ModalImage from "react-modal-image";

const mapStateToProps = state => ({
  ...state.puppy,
  token: state.common.token
});

const mapDispatchToProps = dispatch => ({
  onTabClick: (tab, pager, payload) => dispatch({ type: CHANGE_TAB, tab, pager, payload })
});

const MediaGallery = props => {
  return (
    <div className="card-deck">
      {
        props.puppyMedia.map((item, index) => {
          return (
            <div className=".col-6">
                  <ModalImage
                    className="card-img-top img-gallery"
                    small={item.fileName}
                    large={item.fileName}
                    hideDownload={true}
                    hideZoom={true}
                    alt={"Media_" + index}
                  />
                  {/* <img className="card-img-top img-gallery" src={item.fileName} alt='Filename_placeholder' ></img> */}
            </div>
          );
        })
      }
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MediaGallery);
