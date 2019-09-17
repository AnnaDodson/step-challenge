import React, { Component } from 'react';
import * as moment from 'moment';

const teamId = 1;

async function loadUserSteps(id) {
  const response = await fetch('/graphql', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({query:
      `{
        teamSteps(id:"${id}"){stepCount, dateOfSteps},
        team (teamId: "${id}"){name}
        }`
    })
  });
  const responseBody = await response.json();
  return responseBody;
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
    var dateOfSteps = moment(steps[i].dateOfSteps).format('MMM Do');
    children.push(<td key={'day_' + i}>{stepCount}<br/>{dateOfSteps}</td>)

    if(children.length === 8){
      table.push(<tr key={'col_' + i}>{children}</tr>)
      children = []
      week = week +1;
    }
  }

  return table
}

export class TeamScoreboard extends Component {
  static displayName = TeamScoreboard.name;

  constructor (props) {
    super(props);
    this.state = { 
      steps: [],
      showTable: false,
       teamName:'',
    };
    this.createTable = createTable;
  }

  componentDidMount() {
    loadUserSteps(teamId).then(res =>
      this.setState({
        steps : res.teamSteps,
        teamName:res.team.name,
        showTable: true,
      }))
  }

  render () {
    const teamName = this.state.teamName;
    const steps = this.state.steps;
    const showTable = this.state.showTable;
    return (
      <div>
      <div>
        <h2>{ teamName }</h2>
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
