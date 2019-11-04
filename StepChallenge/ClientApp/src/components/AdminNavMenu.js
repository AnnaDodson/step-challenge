import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import Auth from './Auth';
import './NavMenu.css';

export class AdminNavMenu extends Component {
  static displayName = AdminNavMenu.name;

  constructor (props) {
    super(props);
    this.auth = new Auth();
    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      isLoggedIn : this.auth.isLoggedIn()
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    return (
      <header>
        <Navbar className="admin-container navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/admin">Admin</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            {this.state.isLoggedIn &&
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/admin/participants">Participants</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/admin/teams">Teams</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/admin/settings">Settings</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/admin/leaderboard">Leader Board</NavLink>
                </NavItem>
              </ul>
            </Collapse>
            }
          </Container>
        </Navbar>
      </header>
    );
  }
}