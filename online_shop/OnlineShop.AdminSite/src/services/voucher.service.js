import { APIClient } from 'core/helpers/api_helper';
const baseUrl = `api/vouchers`;
const api = new APIClient();

export const Get = (params) => api.get(baseUrl + '/list', params);
// export const Create = (params) => api.create(baseUrl, params);
export const Delete = (params) => api.delete(baseUrl + "/delete", params);
// export const UpdateInfo = (params) => api.create(baseUrl, params);
// export const UpdateAvatar = (params) => api.create(baseUrl + '/change-avatar', params);
// export const ChangePassword = (params) => api.create(baseUrl + '/change-password', params);
// export const GetCurrentUser = () => {
//   return api.get(baseUrl + '/get-current-user');
// };
