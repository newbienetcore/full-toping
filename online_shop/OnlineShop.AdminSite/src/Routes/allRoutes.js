
import React from 'react';
import { Redirect } from 'react-router-dom';
import Loadable from 'components/Loadable';
import { lazy } from 'react';
import Login from 'pages/Authentication/Login';
import RolePage from 'pages/role/index';

const UserPage = Loadable(lazy(() => import('pages/user')));
const VoucherPage = Loadable(lazy(() => import('pages/voucher')));

const authProtectedRoutes = [
  { path: '/users', component: UserPage },
  { path: '/vouchers', component: VoucherPage },
  { path: '/roles', component: RolePage },
  {
    path: '/',
    exact: true,
    component: () => <Redirect to="/users" />
  }
];

const publicRoutes = [{ path: '/login', component: Login }];

export { authProtectedRoutes, publicRoutes };
