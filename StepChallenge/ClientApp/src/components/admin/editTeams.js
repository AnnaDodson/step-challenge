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

async function updateTeam(newDetails){
  const response = await fetch('/graphql', {
   method:'POST',
   headers:{'content-type':'application/json'},
   body: JSON.stringify({
       "query": ` mutation updateTeam { updateTeam ( team : {  teamId : ${newDetails.teamId}, teamName : "${newDetails.teamName}", numberOfParticipants : ${newDetails.numberOfParticipants} } )  { teamName , numberOfParticipants  } } `
      })
  })
  const responseBody = await response.json();
  var result = null;
  if(responseBody.hasOwnProperty("updateTeam")){
      result = responseBody.updateTeam;
  }
  return result;
}

export class EditTeams extends Component {
  static displayName = EditTeams.name;

  constructor (props) {
    super(props);
    this.state = {
        loading: true,
        error: false,
        teams : [],
        team : {},
        showTeam : false,
        teamName : "",
        numberOfParticipants: 0,
        teamId : 0,
        error : false,
        saved : false,
    }
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleChangeTeam = this.handleChangeTeam.bind(this);
    this.handleChangeName = this.handleChangeName.bind(this);
    this.handleChangeNumberOfParticipants = this.handleChangeNumberOfParticipants.bind(this);
    var query = `{ "query": "{ teams { teamName, teamId, numberOfParticipants, participants { participantName } } }" }`
    this.apiHelper = new ApiHelper();
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
      if(data.hasOwnProperty("teams")){
        this.setState({
            teams: data.teams,
            loading: false
        });
      }else{
        this.setState({
            error: true,
            loading: false
        });
      }
    })
  }

  handleChangeTeam(event) {
    this.setState({ showTeam : false, saved : false })
    var selectedTeamId = event.target.value
    var selectedTeam =_.find(this.state.teams, function(team){ return team.teamId == selectedTeamId; }); 
    if(selectedTeam){
      this.setState({
        team: selectedTeam,
        teamName: selectedTeam.teamName,
        teamId: selectedTeam.teamId,
        numberOfParticipants: selectedTeam.numberOfParticipants,
        showTeam : true})
    }
  }

  handleChangeName(event){
    this.setState({teamName: event.target.value, saved: false});
  }

  handleChangeNumberOfParticipants(event){
    this.setState({numberOfParticipants: event.target.value, saved : false});
  }

  handleSubmit(event){
    var self = this;
    event.preventDefault();
    var updated = {
        teamId : this.state.teamId,
        teamName : this.state.teamName,
        numberOfParticipants : this.state.numberOfParticipants
    }
    updateTeam(updated).then(data => {
        var findTeam =_.find(this.state.teams, function(team){ return team.teamId == updated.teamId }); 
        findTeam.numberOfParticipants = data.numberOfParticipants
        findTeam.teamName = data.teamName
        this.setState({
          numberOfParticipants: data.numberOfParticipants,
          teamName: data.teamName,
          saved : true
        });
    })

  }

  render () {
    var display = !this.state.loading && !this.state.error
    return (
        <div>
          {this.state.loading &&
            <p><em>Loading...</em></p>
          }
          <div className="row">
          {display &&
            <div className="col-md-6">
                <div>
                    <label>
                    Teams
                    </label>
                    <br />
                    <select key={"teams"} style={{"width": "60%"}} value={this.state.team} onChange={this.handleChangeTeam}>
                      <option value='0' defaultValue>Choose a team</option>
                      {this.state.teams.map(team =>
                        <option key={team.teamId} value={team.teamId}>{ team.teamName }</option>
                      )}
                    </select>
                </div>
            </div>
          }
          {this.state.showTeam &&
            <div className="col-md-6">
              <form style={formStyle} onSubmit={this.handleSubmit} key="edit">
                <label><strong>Team Name:</strong></label>
                <br />
                <input type="text" value={this.state.teamName} onChange={this.handleChangeName} />
                <br />
                <br />
                <label><strong>Number of Participants:</strong></label>
                <br />
                <select key={"numberOfParticipants"} value={this.state.numberOfParticipants} onChange={this.handleChangeNumberOfParticipants}>
                    <option key={0} value={0}>0</option>
                    <option key={1} value={1}>1</option>
                    <option key={2} value={2}>2</option>
                    <option key={3} value={3}>3</option>
                    <option key={4} value={4}>4</option>
                    <option key={5} value={5}>5</option>
                    <option key={6} value={6}>6</option>
                    <option key={7} value={7}>7</option>
                    <option key={8} value={8}>8</option>
                    <option key={9} value={9}>9</option>
                    <option key={10} value={10}>10</option>
                </select>
                <br />
                <br />
                <p><strong>Participants:</strong></p>
                <ol>
                  {this.state.team.participants.map(participant =>
                      <li>{participant.participantName}</li>
                  )}
                </ol>
                <br />
                <input className="btn btn-success" type="submit" disabled={this.state.error} value="Save" />
                {this.state.saved &&
                <p style={{"color":"green"}}>Saved</p>
                }
              </form>
          </div>
          }
          {this.state.error &&
            <div>
              <h4>Something unexpected has happened. Try again later</h4>
            </div>
          }
        </div>
        </div>
    );
}}

