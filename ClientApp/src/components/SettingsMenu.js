import React, { Component } from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import Auth from './Auth';

export default class SettingsMenu extends Component {
  static displayName = SettingsMenu.name;
  constructor (props) {
    super(props);
    this.state = {
    };
    this.handleLogout = this.handleLogout.bind(this);
    this.auth = new Auth()
  }

  render () {
    return (
        <Dropdown>
            <Dropdown.Toggle variant="Secondary" id="dropdown-basic">
              Settings
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item>Account</Dropdown.Item>
              <Dropdown.Item onClick={this.handleLogout}>Log out</Dropdown.Item>
            </Dropdown.Menu>
        </Dropdown>
    );
  }

  handleLogout(event){
    this.auth.logout();
  }
}