import React, { Component } from 'react';

async function getTeams() {
  const response = await fetch('/api/register/get_teams', {
     method:'GET',
     headers:{'content-type':'application/json'},
  });
  const responseBody = await response.json();
  return responseBody;
}

async function registerUser(username, password, team){
    const response = await fetch('/api/register', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({
      username: username,
      password: password,
      teamId: parseInt(team, 10)
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
        username : '',
        password : '',
        team : null,
        error : null,
    }
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleChangeUsername = this.handleChangeUsername.bind(this);
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
                  Username
                  <input type="text" value={this.state.username} onChange={this.handleChangeUsername} />
                </label>
                <br />
                <label>
                  Password
                  <input type="password" value={this.state.password} onChange={this.handleChangePassword} />
                </label>
                <br />

                <label>
                  Team
                  <select value={this.state.team} onChange={this.handleChangeTeam}>
                    <option value="null "></option>
                    {teams.map(team =>
                      <option value={team.teamId}>{ team.name }</option>
                    )}
                  </select>
                </label>

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

  handleChangeUsername(event) {
    this.setState({error: null});
    this.setState({username: event.target.value});
  }

  handleChangeTeam(event) {
    this.setState({error: null});
    this.setState({team: event.target.value});
  }

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    registerUser(this.state.username, this.state.password, this.state.team).then(function(res){
        if(res.error){
            self.setState({error: res.error});
        }
    })
  }
}
