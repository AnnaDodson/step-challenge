import React, { Component } from 'react';
import * as moment from 'moment';
import ApiHelper from '../ApiHelper';

export class TotalOverview extends Component {
  static displayName = TotalOverview.name;

  constructor (props) {
    super(props);
    this.state = {
        loading: true,
        error : false,
        teams: [],
    }
    var query = `{ "query": "{ adminParticipantsOverview { teamName, numberOfParticipants, teamTotalSteps, participantsStepsOverviews { participantName, stepTotal, stepsOverviews {stepCount, dateOfSteps} } } }" }`
    this.apiHelper = new ApiHelper();
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
        if(data.hasOwnProperty("adminParticipantsOverview")){
          this.setState({
              loading: false,
              teams : data.adminParticipantsOverview 
          });
        }else{
          this.setState({
              loading: false,
              error: true
          });
        }
    })
  }

  static getStepTds (steps){
    var firstParticipantRow = []
    steps.forEach(step => {
      firstParticipantRow.push(<td >{step.stepCount}</td>)
    });
    return firstParticipantRow
  }

  static renderTable (teams) {
    //TODO get from api
    let totalDays = 81
    var startDate = moment("2019-09-16T02:00:00+00:00");
    var formattedDayHeader = [];
    for (let index = 0; index < totalDays; index++) {
      var day = moment(startDate).add(index, 'days').format('D-MM');
      formattedDayHeader.push(<th key={day}>{ day }</th>);
    }
    /** Header */
    const header = (<tr>
      <th>Team Name</th>
      <th>Participants Name</th>
      {formattedDayHeader}
      <th>Participant Total</th>
      <th>Team Total</th>
    </tr>)
    /** Header */
    const tableRows = [];
    teams.forEach(team => {
      for (let i= 0; i< team.numberOfParticipants; i++) {
        var row = []
        var participant = team.participantsStepsOverviews[i] ? team.participantsStepsOverviews[i] : { participantName : "Not known", stepTotal : 0, stepsOverviews : new Array(totalDays).fill({ stepCount : 0 }) }
        if(i === 0){
          var firstParticipantRow = TotalOverview.getStepTds(participant.stepsOverviews)
          row.push(
            <tr key={team.teamName + "-" + participant.participantName}>
              <th key={team.teamName} rowSpan={team.numberOfParticipants}>{team.teamName}</th>
              <td key={participant.participantName}>{ participant.participantName }</td>
              {firstParticipantRow}
              <td>{participant.stepTotal}</td>
              <th key={team.teamName + "_total"} rowSpan={team.numberOfParticipants}>{team.teamTotalSteps}</th>
            </tr>
          )
        }
        else{
          var participantRow = TotalOverview.getStepTds(participant.stepsOverviews)
          row.push(
            <tr key={team.teamName + "-" + participant.participantName}>
              <td key={participant.participantName + "-" + i}>{ participant.participantName }</td>
              {participantRow}
              <td>{participant.stepTotal}</td>
            </tr>)
        }
        console.log(row)
        tableRows.push(row)
      }
    })
    return(
      <table><tbody>{header}{tableRows}</tbody></table>
    )
  }

  render () {
      let contents = this.state.loading ?
          <p><em>Loading...</em></p> :
          TotalOverview.renderTable(this.state.teams)
    return (
      <div className="table">
        {contents}
      </div>
    )
}}
