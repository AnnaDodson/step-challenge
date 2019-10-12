import React, { Component } from 'react';
import ApiHelper from './ApiHelper';
import CreateStepTable from './CreateStepTable';

const teamId = 1;

async function loadUserSteps(id) {
  var apiHelper = new ApiHelper()
  var query = `{"query": "query teamQuery( $teamId : ID! )
     { teamSteps( teamId : $teamId )
        {stepCount, dateOfSteps, participantsStepsStatus { participantName, participantAddedStepCount} }, 
        team (teamId: $teamId )
        {teamName, numberOfParticipants, participants { participantName }}
      } ",
    "variables": {
     "teamId": "0"
    }
  }`
  const responseBody = await apiHelper.GraphQlApiHelper(query);
  return responseBody;
}

export class TeamScoreboard extends Component {
  static displayName = TeamScoreboard.name;

  constructor (props) {
    super(props);
    this.state = { 
      steps: [],
      loading: true,
      teamName:'',
      error: false
    };
  }

  componentDidMount() {
    loadUserSteps(teamId).then(res => {
      if(res.hasOwnProperty("teamSteps")){
        this.setState({
          steps : res.teamSteps,
          teamName:res.team.teamName,
          teamNumberOfParticipants :res.team.numberOfParticipants,
          participants : res.team.participants,
          loading: false,
        })
      }else{
        this.setState({
            error: true,
            loading: false
        });
      }})
  }

  static renderTeamScores (steps, participants, teamName, teamNumberOfParticipants, error) {
    if (error){
      return (
        <div>
          <p>Something went terribly terribly wrong</p>
        </div>
      )
    }else{
      return (
        <div>
          <h3>{ teamName }</h3>
          <p>
          {participants.map(participant =>
            <span>{participant.participantName}, </span>
          )}
          </p>
          < CreateStepTable steps={steps} numberOfParticipants={teamNumberOfParticipants} table={"team"} />
        </div>
        )
    }
  }

  render () {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : TeamScoreboard.renderTeamScores(this.state.steps, this.state.participants, this.state.teamName, this.state.teamNumberOfParticipants, this.state.error);
    
    return (
      <div>
        <h2>Team Scores</h2>
        {contents}
      </div>
    );
  }
}
