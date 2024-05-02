import axios from 'axios';
let isRefreshing = false;
let failedQueue = [];

let current_user = localStorage.getItem('current_user');
current_user = JSON.parse(current_user);
const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL + '/',
  headers: {
    'Content-Type': 'application/json',
    Authorization: current_user && current_user.accessToken ? `Bearer ${current_user.accessToken}` : ''
  }
});

api.defaults.headers.post['Content-Type'] = 'application/json';

let blackListUrl = ['auth/login', 'auth/refresh-token'];
api.interceptors.request.use(function (req) {
  req.withCredentials = true;
  return req;
});

const processQueue = (error, token = null) => {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });

  failedQueue = [];
};

api.interceptors.response.use(
  (res) => {
    return res.data ? res.data : res;
  },
  async (err) => {
    const originalRequest = err.config;
    const isblacklist = originalRequest ? blackListUrl.includes(originalRequest.url) : true;
    if (!isblacklist && err.response) {
      if (err.response.status === 401 && !originalRequest._retry) {
        if (isRefreshing) {
          try {
            const token = await new Promise(function (resolve, reject) {
              failedQueue.push({ resolve, reject });
            });
            originalRequest.headers['Authorization'] = token;
            return api(originalRequest);
          } catch (err_1) {
            return await Promise.reject(err_1);
          }
        }

        originalRequest._retry = true;
        isRefreshing = true;
        return new Promise(function (resolve, reject) {
          api
            .get('api/auth/refresh-token')
            .then((rs) => {
              originalRequest.headers['Authorization'] = `${rs}`;
              processQueue(null, rs);
              resolve(api(originalRequest));
            })
            .catch((err) => {
              processQueue(err, null);
              reject(err);
            })
            .then(() => {
              isRefreshing = false;
            });
        });
      }
    }
    return Promise.reject(err);
  }
);

class APIClient {
  get = (url, params) => {
    let response;
    let paramKeys = [];
    if (params) {
      Object.keys(params).map((key) => {
        paramKeys.push(key + '=' + params[key]);
        return paramKeys;
      });
      const queryString = paramKeys && paramKeys.length ? paramKeys.join('&') : '';
      // eslint-disable-next-line no-debugger
      debugger;
      response = api.get(`${url}?${queryString}`, params);
    } else {
      // eslint-disable-next-line no-debugger
      debugger;
      response = api.get(`${url}`);
    }
    return response;
  };

  create = (url, data, config) => {
    return api.post(url, data, config);
  };

  delete = (url, id) => {
    return api.delete(url + '/' + id);
  };
}

export { APIClient };
