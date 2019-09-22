import React, { Component } from 'react'
import ApiHelper from './ApiHelper';
import CreateStepTable from './CreateStepTable';

async function loadUserSteps(id) {
 var query = `{
  "query": "query participantQuery( $participantId : ID!) { participant( participantId : $participantId) { participantName, team {teamName}, steps {stepCount, dateOfSteps, week, day}}}",
  "variables": {
   "participantId": "null" }
  }`
  var apiHelper = new ApiHelper();
  const response = await apiHelper.GraphQlApiHelper(query);
  return response.participant;
}

class UserSteps extends Component {
  constructor(props) {
     super(props);
     this.userId = props.userId
     this.state =  {
       teamName:'',
       steps: [],
       userName : '',
       loading: true
      }
  }

  componentDidMount() {
    loadUserSteps(this.userId).then(res =>
      this.setState({
        teamName:res.team.teamName,
        userName: res.participantName,
        steps : res.steps,
        loading: false,
      }))
  }
  

  static renderUserStepTable (steps) {
    return (
        <div>
          < CreateStepTable data={steps} table={"user"} />
        </div>
        )
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : UserSteps.renderUserStepTable(this.state.steps);

    return (
      <div>
        <h2>You Steps</h2>
          {contents}
      </div>
    );
  }
}

export default UserSteps;