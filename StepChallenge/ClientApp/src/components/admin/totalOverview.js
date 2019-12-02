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
        highestStepsParticipant: 0,
        highestStepsParticipantId: 0,
        highestStepsTeam: 0,
        highestStepsTeamId: 0,
        teams: [],
    }
    var query = `{ "query": "{ adminParticipantsOverview { highestStepsParticipant, highestStepsParticipantId, highestStepsTeam, highestStepsTeamId,
      teams { teamId, teamName, numberOfParticipants, teamTotalSteps, teamTotalStepsWithAverage, participantsStepsOverviews { participantId, participantName, stepTotal, stepsOverviews {stepCount, dateOfSteps } } } } }" }`
    this.apiHelper = new ApiHelper();
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
        if(data.hasOwnProperty("adminParticipantsOverview")){
          this.setState({
              loading: false,
              teams : data.adminParticipantsOverview.teams,
              highestStepsParticipant : data.adminParticipantsOverview.highestStepsParticipant,
              highestStepsParticipantId : data.adminParticipantsOverview.highestStepsParticipantId,
              highestStepsTeam : data.adminParticipantsOverview.highestStepsTeam,
              highestStepsTeamId : data.adminParticipantsOverview.highestStepsTeamId,
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

  static renderTable (teams, highest) {
    //TODO get from api
    var startDate = moment("2019-09-16T02:00:00+00:00");
    var endDate = moment("2019-12-01T01:00:00+01:00")
    var totalDays = endDate.diff(startDate, 'days') + 2; //the two is for the two extra columns at the start. Needs sorting out!
    var formattedDayHeader = [];
    for (let index = 0; index < totalDays; index++) {
      var day = moment(startDate).add(index, 'days').format('D-MM');
      formattedDayHeader.push(<th key={day}>{ day }</th>);
    }
    /** Header */
    const header = (<tr>
      <th className="headcol">Team Name</th>
      <th className="headcol headcol2 border-right">Participants Name</th>
      {formattedDayHeader}
      <th className="border-right border-left">Participant Total</th>
      <th>Actual Team Total</th>
      <th className="border-left">Team Total Including Averaged Person</th>
    </tr>)
    /** Header */
    const tableRows = [];
    teams.forEach(team => {
      for (let i= 0; i< team.numberOfParticipants; i++) {
        var row = []
        var participant = team.participantsStepsOverviews[i] ? team.participantsStepsOverviews[i] : { participantName : "Not known", stepTotal : 0, stepsOverviews : new Array(totalDays).fill({ stepCount : 0 }) }
        participant.stepsOverviews = participant.stepsOverviews.length > 0 ? participant.stepsOverviews : new Array(totalDays).fill({ stepCount : 0 })
        if(i === 0){
          var firstParticipantRow = TotalOverview.getStepTds(participant.stepsOverviews)
          row.push(
            <tr key={team.teamName + "-" + participant.participantName}>
              <th className={highest.highestStepsTeamId === team.teamId ? "winner headcol" : "headcol" } key={team.teamId} rowSpan={team.numberOfParticipants}>{team.teamName}</th>
              <td className={highest.highestStepsParticipantId === parseInt(participant.participantId) ? "winner headcol headcol2 border-right border-left" : "headcol headcol2 border-right border-left" } key={participant.participantName}>{ participant.participantName }</td>
              {firstParticipantRow}
              <td className="border-right border-left">{participant.stepTotal}</td>
              <th key={team.teamName + "_total"} rowSpan={team.numberOfParticipants}>{team.teamTotalSteps}</th>
              <th className="border-left" key={team.teamName + "_total_with_average"} rowSpan={team.numberOfParticipants}>{team.teamTotalStepsWithAverage == 0 ? team.teamTotalSteps : team.teamTotalStepsWithAverage}</th>
            </tr>
          )
        }
        else{
          var participantRow = TotalOverview.getStepTds(participant.stepsOverviews)
          row.push(
            <tr key={team.teamName + "-" + participant.participantName}>
              <td className={highest.highestStepsParticipantId === participant.participantId ? "winner headcol headcol2 border-right border-left" : "headcol headcol2 border-left border-right" } key={participant.participantName + "-" + i}>{ participant.participantName }</td>
              {participantRow}
              <td className="border-right border-left">{participant.stepTotal}</td>
            </tr>)
        }
        tableRows.push(row)
      }
    })
    return(
      <div className="overview-table">
      <table className="table table-striped table-hover"><tbody>{header}{tableRows}</tbody></table>
      </div>
    )
  }

  render () {
      let contents = this.state.loading ?
          <p><em>Loading...</em></p> :
          TotalOverview.renderTable(this.state.teams, {highestStepsTeamId : this.state.highestStepsTeamId, highestStepsParticipantId : this.state.highestStepsParticipantId})

    return (
      <div className="table-container">
        {contents}
      </div>
    )
}}
