import React from 'react';
import { connect } from 'react-redux';
import { CHANGE_TAB } from '../../constants/actionTypes';

// const YourFeedTab = props => {
//   if (props.token) {
//     const clickHandler = ev => {
//       ev.preventDefault();
//       props.onTabClick('feed', agent.Search.getMine, agent.Search.getMine());
//     }

//     return (
//       <li className="nav-item">
//         <a  href=""
//             className={ props.tab === 'feed' ? 'nav-link active' : 'nav-link' }
//             onClick={clickHandler}>
//           Your puppies
//         </a>
//       </li>
//     );
//   }
//   return null;
// };

// const GlobalFeedTab = props => {
//   const clickHandler = ev => {
//     ev.preventDefault();
//     props.onTabClick('all', agent.Search.getAll, agent.Search.getAll());
//   };
//   return (
//     <li className="nav-item">
//       <a
//         href=""
//         className={ props.tab === 'all' ? 'nav-link active' : 'nav-link' }
//         onClick={clickHandler}>
//         Global puppies
//       </a>
//     </li>
//   );
// };

const mapStateToProps = state => ({
  ...state.puppy,
  token: state.common.token
});

const mapDispatchToProps = dispatch => ({
  onTabClick: (tab, pager, payload) => dispatch({ type: CHANGE_TAB, tab, pager, payload })
});

const handleClick = ev => {
  ev.preventDefault();
  alert("OK");
};

const MediaGallery = props => {
  return (
    <div className="card-deck">
      {
        props.puppyMedia.map(item => {
          return (
            <div className=".col-6">
              <div className="card">
                <a href={`#`}>
                  <img className="card-img-top img-gallery" src={item.fileName} alt='Filename_placeholder' ></img>
                </a>
                <div className="row">
                  <div className="col-md-6 gallery-padding">
                    <div className="float-right">
                      <button className="btn btn-sm btn-gallery" onClick={handleClick}>
                        <i className="ion-heart"> 2</i>
                      </button>
                    </div>
                  </div>
                  <div className="col-md-6 gallery-padding">
                    <button className="btn btn-sm btn-gallery" onClick={handleClick}>
                      <i className="ion-ios-chatboxes"> 2</i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          );
        })
      }
    </div>
  );
};

export default connect(mapStateToProps, mapDispatchToProps)(MediaGallery);
