import superagentPromise from 'superagent-promise';
import _superagent from 'superagent';

const superagent = superagentPromise(_superagent, global.Promise);

const API_ROOT = 'http://192.168.1.16:30902/';

const encode = encodeURIComponent;
const responseBody = res => res.body;

let token = null;
const tokenPlugin = req => {
  if (token) {
    req.set('authorization', `Bearer ${token}`);
  }
}

const requests = {
  del: url =>
    superagent.del(`${API_ROOT}${url}`).use(tokenPlugin).then(responseBody),
  get: url =>
    superagent.get(`${API_ROOT}${url}`).use(tokenPlugin).then(responseBody),
  put: (url, body) =>
    superagent.put(`${API_ROOT}${url}`, body).use(tokenPlugin).then(responseBody),
  post: (url, body) =>
    superagent.post(`${API_ROOT}${url}`, body).use(tokenPlugin).then(responseBody)
};

const requests_search = {
  get: url =>
    superagent.get(`${API_ROOT}search/${url}`).use(tokenPlugin).then(responseBody)
};

const requests_distribution = {
  get: url =>
    superagent.get(`${API_ROOT}distribution/${url}`).use(tokenPlugin).then(responseBody),
  post: (url, body) =>
    superagent.post(`${API_ROOT}distribution/${url}`, body).use(tokenPlugin).then(responseBody)
};
const Search = {
  getAll: () =>
  requests_search.get('api/search/all'),
  getMine: () =>
  requests_search.get('api/search/mine')
};

const Auth = {
  current: () =>
    requests.get('auth/api/user'),
  login: (username, password) =>
    requests.post('auth/api/user/login', {  username, password }),
  register: (username, email, password) =>
    requests.post('auth/api/user/register', { username, email, password } ),
  save: user =>
    requests.put('auth/api/user', { user })
};

const Tags = {
  getAll: () => requests.get('/tags')
};

const limit = (count, p) => `limit=${count}&offset=${p ? p * count : 0}`;
const omitSlug = article => Object.assign({}, article, { slug: undefined })
const Articles = {
  all: page =>
    requests.get(`/articles?${limit(10, page)}`),
  byAuthor: (author, page) =>
    requests.get(`/articles?author=${encode(author)}&${limit(5, page)}`),
  byTag: (tag, page) =>
    requests.get(`/articles?tag=${encode(tag)}&${limit(10, page)}`),
  del: slug =>
    requests.del(`/articles/${slug}`),
  favorite: slug =>
    requests.post(`/articles/${slug}/favorite`),
  favoritedBy: (author, page) =>
    requests.get(`/articles?favorited=${encode(author)}&${limit(5, page)}`),
  feed: () =>
    requests.get('/articles/feed?limit=10&offset=0'),
  get: slug =>
    requests.get(`/articles/${slug}`),
  unfavorite: slug =>
    requests.del(`/articles/${slug}/favorite`),
  update: article =>
    requests.put(`/articles/${article.slug}`, { article: omitSlug(article) }),
  create: article =>
    requests.post('/articles', { article })
};

const Puppies = {
  get: id =>
    requests_distribution.get(`api/animal/${id}`),
  add: puppy =>
    requests_distribution.post(`api/animal`, { alias: puppy.alias, description: puppy.description, localization: {country: puppy.country, city: puppy.city, street: puppy.street}, animalType: puppy.type, body: puppy.body, tagList: puppy.tagList })
}

const Media = {
  get: id =>
  requests_distribution.get(`api/media/${id}`),
  add: media =>
  requests_distribution.post(`api/media/profile`, media)
}

const Comments = {
  create: (slug, comment) =>
    requests.post(`/articles/${slug}/comments`, { comment }),
  delete: (slug, commentId) =>
    requests.del(`/articles/${slug}/comments/${commentId}`),
  forArticle: slug =>
    requests.get(`/articles/${slug}/comments`)
};

const Profile = {
  follow: username =>
    requests.post(`/profiles/${username}/follow`),
  get: username =>
    requests.get(`/profiles/${username}`),
  unfollow: username =>
    requests.del(`/profiles/${username}/follow`)
};

export default {
  Articles,
  Auth,
  Comments,
  Profile,
  Tags,
  Search,
  Puppies,
  Media,
  setToken: _token => { token = _token; }
};
