import React, { Component } from 'react';
import ApiHelper from './ApiHelper';

async function loadUsers() {
 var query = `{ "query": "query usersQuery { users { isAdmin, participantName, username, participantId, team {teamName} } }" }`
  var apiHelper = new ApiHelper();
  const response = await apiHelper.GraphQlApiHelper(query);
  return response.users;
}

async function editUser(user){
    const response = await fetch('/api/user/edit_user', {
       method:'POST',
       headers:{'content-type':'application/json'},
       body:JSON.stringify({
        participantId: user.participantId,
        password: user.password,
        isAdmin: user.participantAdmin
      })
    })
    var result = await response.json();
    return result;
}

const formStyle = {
  input : {
    width: '60%',
  },
  select :{
    width: '60%',
  }
};

const errorStyle = {
  color: 'red',
  weight: 700,
};

const successStyle = {
  color: 'green',
  weight: 700,
};


export class Admin extends Component {
  static displayName = Admin.name;

  constructor(props) {
    super(props);
    this.state =  {
        users : [],
        loading: true,
        editing: false,
        editUser : {},
        error : null,
        success : null,
        editParticipantAdmin : false,
        editParticipantPassword : "",
        editParticipantName : "",
        editParticipantUsername : "",
        editParticipantTeamName : "",
        editParticipantId : 0,
    }
    this.handleClick = this.handleClick.bind(this);
    this.handleChangeAdmin = this.handleChangeAdmin.bind(this);
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleClick(user) {
    this.setState({editing: false, success: false});
    this.setState({editUser: user});
    this.setState({editParticipantId: user.participantId});
    this.setState({editParticipantName: user.participantName});
    this.setState({editParticipantUsername: user.username});
    this.setState({editParticipantTeamName: user.team.teamName});
    this.setState({editParticipantEmail: user.participantEmail});
    this.setState({editParticipantAdmin: user.isAdmin});
    this.setState({editParticipantPassword: ""});
    this.setState({editing: true});
  }

  handleChangePassword(event) {
    this.setState({error: null, success: null});
    this.setState({editParticipantPassword : event.target.value});
  }

  handleChangeAdmin(event) {
    var newState = this.state.editParticipantAdmin == true ? false : true;
    this.setState({error: null, success: null});
    this.setState({editParticipantAdmin : newState});
  }

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    editUser( {participantAdmin: this.state.editParticipantAdmin, email: this.state.editParticipantEmail, participantId : this.state.editParticipantId,  participantName : this.state.editParticipantName, password : this.state.editParticipantPassword }).then(function(res){
        if(res.error){
            console.log(res.error);
            self.setState({error: res.error});
        }
        else{
            self.setState({success : "Saved"})
        }
    })
  }

  componentDidMount() {
    loadUsers().then(res =>
      this.setState({
        users : res != null ? res: [],
        loading: false,
      })
    )
  }

  render () {
    return (
        <div>
            {this.state.loading &&
                <p><em>Loading...</em></p>
            }
            <div className="row">
                <div className="col-md-6">
                    {!this.state.loading &&
                        <div>
                          {this.state.users.map(user =>
                            <p><button className="btn btn-link" key={user.participantId} onClick={() => this.handleClick(user)}>{ user.participantName } - { user.username }</button></p>
                          )}
                        </div>
                    }
                </div>
                <div className="col-md-6">
                  {this.state.editing &&
                    <div>
                          <p>Name: {this.state.editParticipantName}</p>
                          <p>Username: {this.state.editParticipantUsername}</p>
                          <p>Team: {this.state.editParticipantTeamName}</p>
                          <br />
                        <form style={formStyle} onSubmit={this.handleSubmit} key="edit">
                          <label>
                            Reset Password
                          </label>
                          <br />
                          <input type="text" type="password" value={this.state.editParticipantPassword} onChange={this.handleChangePassword} />
                          <br />
                          <br />
                          <label>
                            Admin Access
                          </label>
                          <input type="checkbox" checked={this.state.editParticipantAdmin} style={{ marginLeft: "8px"}} onChange={this.handleChangeAdmin} />
                          <br />
                          <br />
                          <input type="submit" disabled={this.state.error} value="Save" />
                        </form>
                        {this.state.error &&
                          <p style={errorStyle}>{this.state.error}</p>
                        }
                        {this.state.success &&
                          <p style={successStyle}>{this.state.success}</p>
                        }
                    </div>
                  }
                </div>
            </div>
        </div>
    );
  }
}