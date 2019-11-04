import React, { Component } from 'react';
import Dropdown from 'react-bootstrap/Dropdown';
import Auth from './Auth';

export default class SettingsMenu extends Component {
  static displayName = SettingsMenu.name;
  constructor (props) {
    super(props);
    this.handleLogout = this.handleLogout.bind(this);
    this.auth = new Auth()
    this.isAdmin = this.auth.isAdmin();
    this.state = {
      isAdmin : this.auth.isAdmin(),
    };
  }

  render () {
    return (
        <Dropdown>
            <Dropdown.Toggle variant="Secondary" id="dropdown-basic">
              Settings
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item href="/account">Account</Dropdown.Item>
              {this.state.isAdmin &&
                <Dropdown.Item href="/admin">Admin</Dropdown.Item>
              }
              <Dropdown.Item onClick={this.handleLogout}>Log out</Dropdown.Item>
            </Dropdown.Menu>
        </Dropdown>
    );
  }

  handleLogout(event){
    this.auth.logout();
  }
}