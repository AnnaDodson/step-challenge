import React, { Component } from 'react'
import * as moment from 'moment';
import UserStep from './UserStep'

const userId = 1;
const startOfChallenge = moment().set({'year': 2019, 'month': 8, 'date': 16});

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

// TODO: this
var getStepCount = (steps, week, day) => {
  var result = 0;
  if(steps.length > 0){
    for(var i = 0; i < steps.length; i++){
      if(steps[i].week === week && steps[i].day === day){
        result = steps[i].stepCount
      }
    }
  }
  return result;
}

var createTable = (steps) => {
  console.log("create table")
  let table = [];
  let weeks = 5; //challenge is for 5 weeks
  let days = ["Week", "Mon", "Tues", "Weds", "Thurs", "Fri", "Sat", "Sun"];
  let header = [];
  for (let i = 0; i < days.length; i++) {
    header.push(<td key={days[i]}>{`${days[i]}`}</td>)
  }
  table.push(<tr key={'header'}>{header}</tr>)
  var week = moment(startOfChallenge);
  for (let j = 1; j <= weeks; j++) {
    let children = []
    children.push(<td key={'week-' +days[j]}>{`${j}`}</td>)
    for (let i = 0; i < ( days.length - 1); i++) {
      var anySteps = getStepCount(steps, j, i+1)
      var data = {
        count : anySteps,
        userId : userId,
        date : moment(week).add(i, 'days'),
      }
      children.push(<td key={days[i]}>
      <UserStep data={data}/>
      </td>)
    }
    week.add(1, 'week')
    table.push(<tr key={'col' +j}>{children}</tr>)
  }
  return table
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
        <h2>{ userName }</h2>
        <h4>{ teamName }</h4>
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