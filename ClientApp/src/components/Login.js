import React, { Component } from 'react';
import Auth from "./Auth";

const errorStyle = {
  color: 'red',
  weight: 700,
};

const formStyle = {
  input : {
    width: '60%',
    marginLeft: '40px'
  },
  select :{
    width: '60%',
  }
};

async function userLogin(username, password) {
    var auth = new Auth();
    var response = await auth.login(username, password)
    return response;
}

export class Login extends Component {
  static displayName = Login.name;
  constructor(props) {
    super(props);
    this.state =  {
        username : '',
        password : '',
        error : null,
    }
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleChangeUsername = this.handleChangeUsername.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  render () {
    return (
        <div>
        <h1>Login</h1>
            <div>
              <form style={formStyle} onSubmit={this.handleSubmit} key="login">
                <label>
                  Email
                </label>
                <br />
                  <input type="text" value={this.state.username} onChange={this.handleChangeUsername} />
                <br />
                <label>
                  Password
                </label>
                <br />
                  <input type="password" value={this.state.password} onChange={this.handleChangePassword} />
                <br />
                <br />
                <input type="submit" data-date="test" value="Submit" />
                <br />
              </form>
              {this.state.error &&
                <p style={errorStyle}>{this.state.error}</p>
              }
              <br />
              <p><a href="/register">Register here</a></p>
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

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    userLogin(this.state.username, this.state.password)
      .then(function(error){
        if(error){
          self.setState({error: error.error ? error.error : "Something went wrong"});
        }
        else{
          //self.props.history.push('/')
          // Force window to refresh so the nav is shown based on the logged in cookie we just set.
          // TODO - should be a way of updating the nav component without refreshing the page?
          window.location.href = "/";
        }
      })
  }
}