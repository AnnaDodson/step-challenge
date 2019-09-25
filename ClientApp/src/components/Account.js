import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class Account extends Component {
  static displayName = Account.name;

  render () {
    return (
      <div>
        <h2>Your Account</h2>
        <br />
        <h3>Oopes this page doesn't really exist yet.</h3>
        <p>This web app is still being built.</p>
        <br />
        <p>You can take a look at the progress on <Link to="https://github.com/AnnaDodson/step-challenge">GitHub</Link>. All contributions welcome.</p>
        <h3>Found a bug?</h3>
        <p>Raise an issue <Link to="https://github.com/AnnaDodson/step-challenge/issues">Here</Link>.</p>
        <h3>Have a request?</h3>
        <p>Make an issue <Link to="https://github.com/AnnaDodson/step-challenge/issues">Here</Link>.</p>
      </div>
    );
  }
}
