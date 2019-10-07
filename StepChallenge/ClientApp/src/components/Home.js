import React, { Component } from 'react';
import UserSteps from './UserSteps'

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
        <UserSteps/>
    );
  }
}
