import React, { Component } from 'react';
import * as moment from 'moment';
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
      let table = [];
      let days = ["Week", "Mon", "Tues", "Weds", "Thurs", "Fri", "Sat", "Sun"];
      let header = [];
      for (let i = 0; i < days.length; i++) {
        header.push(<td key={'header' + days[i]}>{`${days[i]}`}</td>)
      }
      table.push(<tr key={'header'}>{header}</tr>)
      var week = 1;
      let children = []
      for (let i = 0; i < ( steps.length ); i++) {
        if(children.length === 0){
          var key = 'week_' + days[week] + '_' + i;
          children.push(<td key={key}>{week}</td>)
        }
        var dateOfSteps = moment(steps[i].dateOfSteps);
        var stepCount = steps[i].stepCount;
        if(type === "user"){
              var data = {
                count : stepCount,
                userId : 1,
                date : dateOfSteps
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