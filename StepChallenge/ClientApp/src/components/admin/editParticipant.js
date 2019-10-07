import React, { Component } from 'react';
import ApiHelper from '../ApiHelper';
import * as _ from 'underscore';

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

export class EditParticipant extends Component {
  static displayName = EditParticipant.name;

  constructor (props) {
    super(props);
    this.state = {
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
        errorResponse : false
    }
    this.handleClick = this.handleClick.bind(this);
    this.handleChangeAdmin = this.handleChangeAdmin.bind(this);
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.apiHelper = new ApiHelper();
    var query = `{ "query": "query usersQuery { users { isAdmin, participantName, username, participantId, team {teamName} } }" }`
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
      if(data.hasOwnProperty("users")){
        this.setState({ users: data.users, loading: false});
      }else{
        this.setState({ errorResponse: true, loading: false});
      }
    })
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

  handleClick(event) {
    var selectedUserId = event.target.value
    var user =_.find(this.state.users, function(user){ return user.participantId == selectedUserId; }); 

    this.setState({editing: false, success: false});
    this.setState({error: null, success: null});
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
    this.setState({editParticipantAdmin : newState})
  }

  render () {
    var display = !this.state.loading && !this.state.errorResponse
    return (
      <div>
            <div className="row">
                <div className="col-md-6">
                    {display &&
                        <div>
                          <label>
                          Participants
                          </label>
                          <br />
                          <select key={"users"} value={this.state.user} onChange={this.handleClick}>
                            <option value='0' disabled>select a participant</option>
                            {this.state.users.map(user =>
                              <option key={user.participantId} value={user.participantId}>{ user.participantName }</option>
                            )}
                          </select>
                        </div>
                    }
                </div>
                <div className="col-md-6">
                  {this.state.editing &&
                    <div>
                          <p><strong>Name:</strong> {this.state.editParticipantName}</p>
                          <p><strong>Username:</strong> {this.state.editParticipantUsername}</p>
                          <p><strong>Team:</strong> {this.state.editParticipantTeamName}</p>
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

                {this.state.errorResponse &&
                  <div>
                    <h4>Something unexpected has happened. Try again later</h4>
                  </div>
                }
            </div>
      </div>
    );
}}