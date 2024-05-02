export default function authHeader() {
  const obj = JSON.parse(localStorage.getItem('authUser'));

  if (obj?.accessToken) {
    return { Authorization: obj.accessToken };
  } else {
    return {};
  }
}
