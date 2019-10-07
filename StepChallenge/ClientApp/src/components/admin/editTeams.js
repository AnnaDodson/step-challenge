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
        team : null,
        showTeam : false,
        teamName : "",
        numberOfParticipants: 0,
        teamId : 0,
        error : false,
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
    this.setState({ showTeam : false })
    var selectedTeamId = event.target.value
    var selectedTeam =_.find(this.state.teams, function(team){ return team.teamId == selectedTeamId; }); 
    this.setState({team: selectedTeam});
    this.setState({teamName: selectedTeam.teamName});
    this.setState({teamId: selectedTeam.teamId});
    this.setState({numberOfParticipants: selectedTeam.numberOfParticipants});
    this.setState({ showTeam : true })
  }

  handleChangeName(event){
    this.setState({teamName: event.target.value});
  }

  handleChangeNumberOfParticipants(event){
    this.setState({numberOfParticipants: event.target.value});
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
        this.setState({numberOfParticipants: data.numberOfParticipants});
        this.setState({teamName: data.teamName});
    })

  }

  render () {
    var display = !this.state.loading && !this.state.error
    return (
        <div>
          {this.state.loading &&
            <p><em>Loading...</em></p>
          }
          <div class="row">
          {display &&
            <div class="col-md-6">
                <div>
                    <label>
                    Teams
                    </label>
                    <br />
                    <select key={"teams"} value={this.state.team} onChange={this.handleChangeTeam}>
                      <option value='0' disabled>Choose a team</option>
                      {this.state.teams.map(team =>
                        <option key={team.teamId} value={team.teamId}>{ team.teamName }</option>
                      )}
                    </select>
                </div>
            </div>
          }
          {this.state.showTeam &&
            <div class="col-md-6">
              <form style={formStyle} onSubmit={this.handleSubmit} key="edit">
                <label>Team Name</label>
                <br />
                <input type="text" value={this.state.teamName} onChange={this.handleChangeName} />
                <br />
                <br />
                <label>Number of Participants</label>
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
                <input type="submit" disabled={this.state.error} value="Save" />
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

