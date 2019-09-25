import React, { Component } from 'react';
import * as moment from 'moment';
import * as _ from 'underscore';
import UserStep from './UserStep'

class CreateStepTable extends Component{
  constructor(props) {
    super(props);
    this.state =  {
    }
    this.steps = props.data;
    this.type = props.table;
  }

  static renderTable (steps, type) {
      var startDate = moment("2019-09-16T02:00:00+01:00");
      var today = moment();
      var diff = today.diff(startDate, "days");
      var round = Math.ceil(diff/7)*7
      var howManyWeeksToShow = moment(startDate).add(round, 'days');
      var numberOfDays = howManyWeeksToShow.diff(startDate, 'days');
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
      for (let i = 0; i < ( numberOfDays ); i++) {
        if(children.length === 0){
          var key = 'week_' + days[week] + '_' + i;
          children.push(<td key={key}>{week}</td>)
        }
        var dateOfSteps = moment(startDate).add(i, "day");
        // TODO can probably assume the dates are in the correct order here
        var stepCount  = _.find(steps, function(step){
          return moment(step.dateOfSteps).isSame(dateOfSteps, 'day')
        });
        if(stepCount){
          stepCount  = stepCount.stepCount;
        }
        else{
          stepCount = 0;
        }
        totalCount = totalCount + stepCount;

        if(type === "user"){
              var data = {
                count : stepCount,
                userId : 1,
                date : moment(dateOfSteps)
              }
              children.push(<td key={days[i] + '_' + i }>
                <UserStep data={data}/>
              </td>)
        }
        else{
            children.push(<td key={'day_' + i}>{stepCount}<br/>{dateOfSteps.format('MMM Do')}</td>)
        }

        if(children.length === 8){
          table.push(<tr key={'col_' + i}>{children}</tr>)
          children = []
          totalCount = 0;
          week = week +1;
        }
      }

      return table
  }

  render() {
    let contents = CreateStepTable.renderTable(this.steps, this.type);
      return(
          <table className="table">
            <tbody>
                {contents}
            </tbody>
          </table>

      )}
}

export default CreateStepTable;