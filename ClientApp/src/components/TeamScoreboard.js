import React, { Component } from 'react';
import ApiHelper from './ApiHelper';
import CreateStepTable from './CreateStepTable';

const teamId = 1;

async function loadUserSteps(id) {
  var apiHelper = new ApiHelper()
  var query = `{"query": "query teamQuery( $teamId : ID! )
     { teamSteps( teamId : $teamId )
        {stepCount, dateOfSteps}, 
        team (teamId: $teamId )
        {teamName}
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
    };
  }

  componentDidMount() {
    loadUserSteps(teamId).then(res =>
      this.setState({
        steps : res.teamSteps,
        teamName:res.team.name,
        loading: false,
      }))
  }

  static renderTeamScores (steps) {
    return (
        <div>
          < CreateStepTable data={steps} table={"team"} />
        </div>
        )
  }

  render () {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : TeamScoreboard.renderTeamScores(this.state.steps);

    return (
      <div>
        <h2>Team Scores</h2>
        {contents}
      </div>
    );
  }
}
