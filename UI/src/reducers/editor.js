import {
  EDITOR_PAGE_LOADED,
  EDITOR_PAGE_UNLOADED,
  ARTICLE_SUBMITTED,
  ASYNC_START,
  ADD_TAG,
  REMOVE_TAG,
  UPDATE_FIELD_EDITOR,
  ARTICLE_MEDIA_SUBMITTED,
  ON_ERRORS
} from '../constants/actionTypes';

export default (state = {}, action) => {
  switch (action.type) {
    case EDITOR_PAGE_LOADED:
      return {
        ...state,
        animalId: action.payload ? action.payload.animalId : '',
        articleSlug: action.payload ? action.payload.article.slug : '',
        title: action.payload ? action.payload.article.title : '',
        description: action.payload ? action.payload.article.description : '',
        body: action.payload ? action.payload.article.body : '',
        tagInput: '',
        tagList: action.payload ? action.payload.article.tagList : [],
        alias: action.payload ? action.payload.alias : '',
        country: action.payload ? action.payload.country : '',
        city: action.payload ? action.payload.city : '',
        street: action.payload ? action.payload.street : '',
        profile: action.payload ? action.payload.profile : '',
        type: action.payload ? action.payload.type : ''
      };
    case EDITOR_PAGE_UNLOADED:
      return {};
    case ARTICLE_MEDIA_SUBMITTED:
      return {
        ...state,
        inProgress: null,
        errors: action.error ? action.payload.errors : null
      };
    case ARTICLE_SUBMITTED:
      return {
        ...state,
        submitted: true,
        inProgress: null,
        errors: action.error ? action.payload.errors : null,
        animalId: action.error ? null : action.payload.animalId
      };
    case ASYNC_START:
      if (action.subtype === ARTICLE_SUBMITTED) {
        return { ...state, inProgress: true };
      }
      break;
    case ADD_TAG:
      return {
        ...state,
        tagList: state.tagList.concat([state.tagInput]),
        tagInput: ''
      };
    case REMOVE_TAG:
      return {
        ...state,
        tagList: state.tagList.filter(tag => tag !== action.tag)
      };
    case UPDATE_FIELD_EDITOR:
      return { ...state, [action.key]: action.value };
    case ON_ERRORS:
      return { ...state, errors: ['Profile required'] };
    default:
      return state;
  }

  return state;
};
