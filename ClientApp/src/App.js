import React, { Component } from 'react';
//import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { DisplayConditions } from './components/DisplayConditions';
import { DisplayFileData} from './components/DisplayFileData';
import { BrowserRouter as Router, Switch, Route, Link } from 'react-router-dom';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Router>
        <Layout>
          <Route exact path='/' component={DisplayConditions} />
          <Route path='/displayfiledata' component={DisplayFileData} />
        </Layout>  
      </Router>
    );
  }
}
