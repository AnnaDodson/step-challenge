import React, { Component } from 'react';
import ApiHelper from './ApiHelper';

async function loadUsers() {
 var query = `{ "query": "query usersQuery { users { isAdmin, participantName, participantId, email, team {teamName} } }" }`
  var apiHelper = new ApiHelper();
  const response = await apiHelper.GraphQlApiHelper(query);
  return response.users;
}

async function editUser(user){
    const response = await fetch('/api/user/edit_user', {
       method:'POST',
       headers:{'content-type':'application/json'},
       body:JSON.stringify({
        participantName: user.participantName,
        participantId: user.participantId,
        password: user.password,
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
        userId : null,
        loading: true,
        editing: false,
        editUser : {},
        error : null,
        success : null,
        editParticipantName : "",
        editParticipantEmail : "",
        editParticipantPassword : "",
        editParticipantId : 0,

    }
    this.handleClick = this.handleClick.bind(this);
    this.handleChangeName = this.handleChangeName.bind(this);
    this.handleChangeEmail = this.handleChangeEmail.bind(this);
    this.handleChangePassword = this.handleChangePassword.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleClick(event) {
    this.setState({editUser: event});
    this.setState({editParticipantId: event.participantId});
    this.setState({editParticipantName: event.participantName});
    this.setState({editParticipantEmail: event.participantEmail});
    this.setState({editParticipantPassword: ""});
    this.setState({editing: true});
  }

  handleChangePassword(event) {
    this.setState({error: null});
    this.setState({editParticipantPassword : event.target.value});
  }

  handleChangeEmail(event) {
    this.setState({error: null});
    this.setState({editParticipantEmail : event.target.value});
  }

  handleChangeName(event) {
    this.setState({error: null});
    this.setState({editParticipantName : event.target.value});
  }

  handleSubmit(event) {
    var self = this;
    event.preventDefault();
    editUser( {email: this.state.editParticipantEmail, participantId : this.state.editParticipantId,  participantName : this.state.editParticipantName, password : this.state.editParticipantPassword }).then(function(res){
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
                            <p key={user.participantId}><button class="btn btn-light" onClick={() => this.handleClick(user)}>Edit</button>  {user.participantName}</p>
                          )}
                        </div>
                    }
                </div>
                <div className="col-md-6">
                  {this.state.editing &&
                    <div>
                          <p>{this.state.editParticipantName}</p>
                          <br />
                        <form style={formStyle} onSubmit={this.handleSubmit} key="edit">
                          <label>
                            Reset Password
                          </label>
                          <br />
                          <input type="text" type="password" value={this.state.editParticipantPassword} onChange={this.handleChangePassword} />
                          <br />
                          <br />
                          <input type="submit" disabled={this.state.error} value="Submit" />
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