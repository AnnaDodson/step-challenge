import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ScoreBoard } from './components/ScoreBoard';
import { TeamScoreboard } from './components/TeamScoreboard';
import { Register } from './components/Register';
import { Login } from './components/Login';
import { Account } from './components/Account';
import { Admin } from './components/Admin';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout >
        <Route exact path='/' component={Home} />
        <Route path='/team-scoreboard' component={TeamScoreboard} />
        <Route path='/scoreboard' component={ScoreBoard} />
        <Route path='/register' component={Register} />
        <Route path='/login' component={Login} />
        <Route path='/account' component={Account} />
        <Route path='/admin' component={Admin} />
      </Layout>
    );
  }
}
