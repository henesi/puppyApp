import {
  PUPPY_PAGE_LOADED,
  PUPPY_PAGE_UNLOADED
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case PUPPY_PAGE_LOADED:
      return {
        ...state,
        puppy: action.payload[0],
        puppyMedia: action.payload[1]
      };
    case PUPPY_PAGE_UNLOADED:
      return {};
    default:
      return state;
  }
};
