import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ScoreBoard } from './components/ScoreBoard';
import { TeamScoreboard } from './components/TeamScoreboard';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/team-scoreboard' component={TeamScoreboard} />
        <Route path='/fetch-data' component={ScoreBoard} />
      </Layout>
    );
  }
}
