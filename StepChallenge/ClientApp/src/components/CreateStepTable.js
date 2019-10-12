import React, { Component } from 'react';
import * as moment from 'moment';
import * as _ from 'underscore';
import UserStep from './UserStep'
import TeamStep from './TeamStep';


function getDaysToDisplay(startDate){
    var displayFromNextDay = moment().add(2, 'days');
    var diff = displayFromNextDay.diff(startDate, "days");
    var roundUpToTheNextWeek = Math.ceil(diff/7)*7
    var howManyDaysToShow = moment(startDate).add(roundUpToTheNextWeek, 'days');
    return howManyDaysToShow.diff(startDate, 'days');
}

function getTable(steps, numberOfParticipants, type){
    // TODO this date should be saved and returned from the db
    var startDate = moment("2019-09-16T02:00:00+01:00");
    var numberOfDaysToDisplay = getDaysToDisplay(startDate);
    let table = [];
    let days = ["Week", "Mon", "Tues", "Weds", "Thurs", "Fri", "Sat", "Sun"];
    let header = [];
    for (let i = 0; i < days.length; i++) {
      header.push(<td key={'header' + days[i]}>{`${days[i]}`}</td>)
    }
    table.push(<tr key={'header'}>{header}</tr>)
    var week = 1;
    var totalCount = 0;
    let children = []
    for (let i = 0; i < ( numberOfDaysToDisplay ); i++) {
      if(children.length === 0){
        var key = 'week_' + days[week] + '_' + i;
        children.push(<td key={key}>{week}</td>)
      }
      var dateOfSteps = moment(startDate).add(i, "day");
      // TODO can probably assume the dates are in the correct order here
      var stepData  = _.find(steps, function(step){
        return moment(step.dateOfSteps).isSame(dateOfSteps, 'day')
      });
      var stepCount = 0;
      var participantsStatus = []
      if(stepData){
        stepCount  = stepData.stepCount
        participantsStatus = stepData.participantsStepsStatus
      }
      totalCount = totalCount + stepCount;
      var data = {
        count : stepCount,
        date : moment(dateOfSteps)
      }
      if(type === "user"){
            children.push(<td key={days[i] + '_' + i }>
              <UserStep data={data}/>
            </td>)
      }
      if(type === "team"){
        data.participantsStatus = participantsStatus
        data.numberOfParticipants = numberOfParticipants
        children.push(<td key={days[i] + '_' + i }>
          <TeamStep data={data}/>
        </td>)
      }

      if(children.length === 8){
        table.push(<tr key={'col_' + i}>{children}</tr>)
        children = []
        totalCount = 0;
        week = week +1;
      }
    }
    return table;

}

class CreateStepTable extends Component{
  constructor(props) {
    super(props);
    this.state =  {
    }
    this.steps = props.steps;
    this.teamNumberOfParticipants = props.numberOfParticipants ? props.numberOfParticipants : 0;
    this.numberOfParticipants = props.numberOfParticipants;
    this.type = props.table;
  }

  static renderTable (steps, numberOfParticipants, type) {
      return getTable(steps, numberOfParticipants, type)
  }

  render() {
    let contents = CreateStepTable.renderTable(this.steps, this.numberOfParticipants, this.type);
      return(
          <table className="table">
            <tbody>
                {contents}
            </tbody>
          </table>

      )}
}

export default CreateStepTable;