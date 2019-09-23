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

async function registerUser(name, email, password, team){
    const response = await fetch('/api/register', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({
      name: name,
      email: email,
      password: password,
      teamId: team
    })
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
        team : 1,
        error : null,
    }
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleChangeName = this.handleChangeName.bind(this);
    this.handleChangeEmail = this.handleChangeEmail.bind(this);
    this.handleChangeTeam = this.handleChangeTeam.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
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
                  Team
                </label>
                <br />
                  <select key={teams} value={this.state.team} onChange={this.handleChangeTeam}>
                    {teams.map(team =>
                      <option key={team.teamId} value={team.teamId}>{ team.teamName }</option>
                    )}
                  </select>

                <br />
                <br />
                <input type="submit" data-date="test" value="Submit" />
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
  }

  handleChangeEmail(event) {
    this.setState({error: null});
    this.setState({email: event.target.value});
  }

  handleChangeName(event) {
    this.setState({error: null});
    this.setState({name: event.target.value});
  }

  handleChangeTeam(event) {
    this.setState({error: null});
    this.setState({team: event.target.value});
  }

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    registerUser(this.state.name, this.state.email, this.state.password, this.state.team).then(function(res){
        if(res.error){
            self.setState({error: res.error});
        }
        else{
          console.log("registered")
          var auth = new Auth();
          auth.createLoggedInCookie()
          //self.props.history.push('/')
          // Force window to refresh so the nav is shown based on the logged in cookie we just set.
          // TODO - should be a way of updating the nav component without refreshing the page?
          window.location.href = "/";
        }
    })
  }
}
