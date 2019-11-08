import React, { Component } from 'react';
import Auth from './Auth';

async function getTeams() {
  const response = await fetch('/api/register/get_teams', {
     method:'GET',
     headers:{'content-type':'application/json'},
  });
  const responseBody = await response.json();
  return responseBody;
}

const errorStyle = {
  color: 'red',
  weight: 700,
};

const formStyle = {
  input : {
    width: '60%',
  },
  select :{
    width: '60%',
  }
};

export class Register extends Component {
  static displayName = Register.name;

  constructor(props) {
    super(props);
    this.state =  {
        teams : [],
        email : '',
        name : '',
        password : '',
        passwordConfirm : '',
        team : 0,
        error : null,
    }
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleChangePasswordConfirm = this.handleChangePasswordConfirm.bind(this);
    this.handleChangeName = this.handleChangeName.bind(this);
    this.handleChangeEmail = this.handleChangeEmail.bind(this);
    this.handleChangeTeam = this.handleChangeTeam.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.auth = new Auth();
    getTeams()
      .then(data => {
        this.setState({teams: data});
      });
  }

  render () {
    const teams = this.state.teams;
    return (
        <div>
        <h1>Register</h1>
            <div>
              <form style={formStyle} onSubmit={this.handleSubmit} key="register">
                <label>
                  Name
                </label>
                <br />
                <input type="text" value={this.state.name} onChange={this.handleChangeName} />
                <br />
                <label>
                  Email
                </label>
                <br />
                  <input type="text" value={this.state.email} onChange={this.handleChangeEmail} />
                <br />
                <label>
                  Password
                </label>
                <br />
                  <input type="password" value={this.state.password} onChange={this.handleChangePassword} />
                <br />
                <label>
                  Confirm Password
                </label>
                <br />
                  <input type="password" value={this.state.passwordConfirm} onChange={this.handleChangePasswordConfirm} />
                <br />
                <label>
                  Team
                </label>
                <br />
                  <select key={teams} value={this.state.team} onChange={this.handleChangeTeam}>
                    <option value='0' disabled>Choose a team</option>
                    {teams.map(team =>
                      <option key={team.teamId} value={team.teamId}>{ team.teamName }</option>
                    )}
                  </select>

                <br />
                <br />
                <input type="submit" disabled={!this.state.team || this.state.error} data-date="test" value="Submit" />
              </form>
              {this.state.error &&
                <p style={errorStyle}>{this.state.error}</p>
              }
            </div>
        </div>
    );
  }

  handleChangePassword(event) {
    this.setState({error: null});
    this.setState({password: event.target.value});
    if(this.state.password !== event.target.value)
    {
      this.setState({error: "Passwords don't match"});
    }
  }

  handleChangePasswordConfirm(event) {
    this.setState({error: null});
    this.setState({passwordConfirm: event.target.value});
    if(this.state.password !== this.state.passwordConfirm)
    {
      this.setState({error: "Passwords don't match"});
    }
  }

  handleChangeEmail(event) {
    this.setState({error: null});
    this.setState({email: event.target.value});
    if(this.state.password !== this.state.passwordConfirm)
    {
      this.setState({error: "Passwords don't match"});
    }
  }

  handleChangeName(event) {
    this.setState({error: null});
    this.setState({name: event.target.value});
    if(this.state.password !== this.state.passwordConfirm)
    {
      this.setState({error: "Passwords don't match"});
    }
  }

  handleChangeTeam(event) {
    this.setState({error: null});
    this.setState({team: event.target.value});
    if(this.state.password !== this.state.passwordConfirm)
    {
      this.setState({error: "Passwords don't match"});
    }
  }

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    this.auth.registerUser(this.state.name, this.state.email, this.state.password, this.state.team).then(function(res){
        if(res.error){
            self.setState({error: res.error});
        }
        else{
          console.log("registered")
          // TODO - should be a way of updating the nav component without refreshing the page? Also this isn't IE compatible...
          window.location.href = "/";
        }
    })
  }
}
