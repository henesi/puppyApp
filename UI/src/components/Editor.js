import ListErrors from './ListErrors';
import React from 'react';
import agent from '../agent';
import { connect } from 'react-redux';
import {
  ADD_TAG,
  EDITOR_PAGE_LOADED,
  REMOVE_TAG,
  ARTICLE_SUBMITTED,
  EDITOR_PAGE_UNLOADED,
  UPDATE_FIELD_EDITOR,
  ARTICLE_MEDIA_SUBMITTED,
  ON_ERRORS
} from '../constants/actionTypes';

const mapStateToProps = state => ({
  ...state.editor
});

const mapDispatchToProps = dispatch => ({
  onAddTag: () =>
    dispatch({ type: ADD_TAG }),
  onLoad: payload =>
    dispatch({ type: EDITOR_PAGE_LOADED, payload }),
  onRemoveTag: tag =>
    dispatch({ type: REMOVE_TAG, tag }),
  onSubmit: payload =>
    dispatch({ type: ARTICLE_SUBMITTED, payload }),
  onMediaAdd: animalId =>
    dispatch({ type: ARTICLE_MEDIA_SUBMITTED, id: animalId }),
  onUnload: payload =>
    dispatch({ type: EDITOR_PAGE_UNLOADED }),
  onUpdateField: (key, value) =>
    dispatch({ type: UPDATE_FIELD_EDITOR, key, value }),
  onError: () =>
    dispatch({ type: ON_ERRORS }),
});

class Editor extends React.Component {
  constructor() {
    super();

    const updateFieldEvent =
      key => ev => this.props.onUpdateField(key, ev.target.value);
     
    const updateFieldEventMedia =
      key => ev => this.props.onUpdateField(key, ev.target.files[0]);

    this.changeDescription = updateFieldEvent('description');
    this.changeBody = updateFieldEvent('body');
    this.changeTagInput = updateFieldEvent('tagInput');
    this.changeProfile =updateFieldEventMedia('profile'); 
    this.changeAlias = updateFieldEvent('alias');
    this.changeCountry = updateFieldEvent('country');
    this.changeCity = updateFieldEvent('city');
    this.changeStreet = updateFieldEvent('street');
    this.changeType = updateFieldEvent('type');
    this.watchForEnter = ev => {
      if (ev.keyCode === 13) {
        ev.preventDefault();
        this.props.onAddTag();
      }
    };

    this.removeTagHandler = tag => () => {
      this.props.onRemoveTag(tag);
    };

    this.submitForm = ev => {
      ev.preventDefault();
      let article = {
        alias: this.props.alias,
        country: this.props.country,
        city: this.props.city,
        street: this.props.street,
        type: this.props.type,
        description: this.props.body,
        tagList: this.props.tagList
      };

      if(!this.props.profile) {
        // this.props.onUpdateField("errors", "Profile is required!");
        console.log("przed: " + this.props.errors);
        this.props.onError();
        console.log("po: " + this.props.errors);
        return;
      }
      const slug = { slug: this.props.articleSlug };

      const promise = this.props.articleSlug ?
        agent.Articles.update(Object.assign(article, slug)) :
        agent.Puppies.add(article);

      this.props.onSubmit(promise);

      promise.then(res => {
        const data = new FormData();
        data.append("ThumbnailImage", this.props.profile);
        data.append("IsProfile", true);
        data.append("AnimalRef", this.props.animalId);
        const promiseMedia = agent.Media.add(data);
        promiseMedia.then(res2 => {
          this.props.onMediaAdd(this.props.animalId);
        });
      });
    };
  }

  componentWillReceiveProps(nextProps) {
    if (this.props.match.params.slug !== nextProps.match.params.slug) {
      if (nextProps.match.params.slug) {
        this.props.onUnload();
        return this.props.onLoad(agent.Articles.get(this.props.match.params.slug));
      }
      this.props.onLoad(null);
    }
  }

  componentWillMount() {
    if (this.props.match.params.slug) {
      return this.props.onLoad(agent.Articles.get(this.props.match.params.slug));
    }
    this.props.onLoad(null);
  }

  componentWillUnmount() {
    this.props.onUnload();
  }

  render() {
    return (
      <div className="editor-page">
        <div className="container page">
          <div className="row">
            <div className="col-md-10 offset-md-1 col-xs-12">

              <ListErrors errors={this.props.errors}></ListErrors>

              <form>
                <fieldset>
                <div className="row">
                  <div className="col-md-8">
                    <fieldset className="form-group">
                      <input
                        name="profile"
                        className="form-control form-control-lg"
                        type="file"
                        inputProps={{ accept: 'image/*' }}
                        placeholder="Puppy profile"
                        onChange={this.changeProfile} />
                    </fieldset>
                  </div>
                  <div className="col-md-4">
                    <fieldset className="form-group">
                      <input
                        className="form-control form-control-lg"
                        type="text"
                        placeholder="Puppy alias"
                        value={this.props.alias}
                        onChange={this.changeAlias} />
                    </fieldset>
                  </div>
                </div>
                  <div className="row">
                    <div className="col-md-4">
                      <fieldset className="form-group">
                        <input
                          className="form-control"
                          type="text"
                          placeholder="Country"
                          value={this.props.country}
                          onChange={this.changeCountry} />
                      </fieldset>
                    </div>
                    <div className="col-md-4">
                      <fieldset className="form-group">
                        <input
                          className="form-control"
                          type="text"
                          placeholder="City"
                          value={this.props.city}
                          onChange={this.changeCity} />
                      </fieldset>
                    </div>
                    <div className="col-md-4">
                      <fieldset className="form-group">
                        <input
                          className="form-control"
                          type="text"
                          placeholder="Street"
                          value={this.props.street}
                          onChange={this.changeStreet} />
                      </fieldset>
                    </div>
                  </div>

                  <fieldset className="form-group">
                    <textarea
                      className="form-control"
                      rows="8"
                      placeholder="Description"
                      value={this.props.body}
                      onChange={this.changeBody}>
                    </textarea>
                  </fieldset>

                  <fieldset className="form-group">
                    <select
                      defaultValue=''
                      className="form-control"
                      type="number"
                      placeholder="Type"
                      value={this.props.type}
                      onChange={this.changeType}
                      >
                      <option value='' disabled>Select puppy type</option>
                      <option value='1'>Dog</option>
                      <option value='2'>Cat</option>
                      </select>
                  </fieldset>

                  <fieldset className="form-group">
                    <input
                      className="form-control"
                      type="text"
                      placeholder="Tags"
                      value={this.props.tagInput}
                      onChange={this.changeTagInput}
                      onKeyUp={this.watchForEnter} />

                    <div className="tag-list">
                      {
                        (this.props.tagList || []).map(tag => {
                          return (
                            <span className="tag-default tag-pill" key={tag}>
                              <i  className="ion-close-round"
                                  onClick={this.removeTagHandler(tag)}>
                              </i>
                              {tag}
                            </span>
                          );
                        })
                      }
                    </div>
                  </fieldset>

                  <button
                    className="btn btn-lg pull-xs-right btn-primary"
                    type="button"
                    disabled={this.props.inProgress}
                    onClick={this.submitForm}>
                    Create
                  </button>

                </fieldset>
              </form>

            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Editor);
