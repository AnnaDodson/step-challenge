import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import SettingsMenu from './SettingsMenu';
import Auth from './Auth';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

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
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">Step Challenge 2019</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            {this.state.isLoggedIn &&
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/team-scoreboard">Team</NavLink>
                </NavItem>
                <NavItem>
                  <SettingsMenu />
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
