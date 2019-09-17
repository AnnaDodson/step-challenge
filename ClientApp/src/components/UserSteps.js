import React, { Component } from 'react'
import * as moment from 'moment';
import UserStep from './UserStep'

const userId = 1;

async function loadUserSteps(id) {
  const response = await fetch('/graphql', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({query:
      `{user(id:"${id}"){userName, team {name}, steps {stepCount,dateOfSteps, week, day} }}`
    })
  });
  const responseBody = await response.json();
  return responseBody.user;
}

var createTable = (steps) => {
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
    var stepCount = steps[i].stepCount;
    var dateOfSteps = moment(steps[i].dateOfSteps);
      var data = {
        count : stepCount,
        userId : userId,
        date : dateOfSteps
      }
      children.push(<td key={days[i] + '_' + i }>
        <UserStep data={data}/>
      </td>)
    if(children.length === 8){
      table.push(<tr key={'col_' + i}>{children}</tr>)
      children = []
      week = week +1;
    }
  }
  return table;
}

class UserSteps extends Component {
  constructor(props) {
     super(props);
     this.state =  {
       teamName:'',
       steps: [],
       userName : '',
       showTable: false
      }
    this.createTable = createTable;
  }

  componentDidMount() {
    loadUserSteps(userId).then(res =>
      this.setState({
        teamName:res.team.name,
        userName: res.userName,
        steps : res.steps,
        showTable: true,
      }))
  }

  render() {
    const teamName = this.state.teamName;
    const userName = this.state.userName;
    const steps = this.state.steps;
    const showTable = this.state.showTable;
    return (
      <div>
      <div>
        <h2>{ userName } - { teamName }</h2>
      </div>
        {showTable &&
        <div>
          <table className="table">
            <tbody>
              { this.createTable(steps) }
            </tbody>
          </table>
        </div>
        }
      </div>
    );
  }
}

export default UserSteps;