import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Companies } from './components/Companies';
import { Quotes } from './components/Quotes';
import { Chart } from './components/Chart';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import { Registration } from './components/Identity/Registration';
import { Login } from './components/Identity/Login';

import './custom.css'



export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/quotes' component={Quotes} />
        <Route path='/chart' component={Chart} />
        <Route path='/login' component={Login} />
        <Route path='/register' component={Registration} />
        <AuthorizeRoute path='/companies' component={Companies} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
