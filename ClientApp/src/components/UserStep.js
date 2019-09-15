import React, { Component } from 'react'
import * as moment from 'moment';

var userId;

async function setUserSteps(newValue, date){
    var dateISO = moment(date).format();
    console.log(dateISO);
  const response = await fetch('/graphql', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({query:
      `mutation creatStepsEntry {creatStepsEntry ( steps: { stepCount : ${newValue} , dateOfSteps : "${dateISO}", userId : ${userId} }){stepsId}}`
    })
  });
  const responseBody = await response.json();
  return responseBody.user;
}

class UserStep extends Component {
  constructor(props) {
    super(props);
    userId = props.data.userId;
    var formatDate = moment(props.data.date).format('MMMM Do');
    this.state =  {
       step: props.data,
       steps: props.data.count,
       formattedDate: formatDate,
       date: props.data.date,
       value : 0,
    }
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  render() {
      return(
        <div>
          <span>{this.state.steps}</span>
          <form onSubmit={this.handleSubmit} key={this.state.steps}>
            <label>
              <input type="text" value={this.state.value} onChange={this.handleChange} />
            </label>
            <input type="submit" data-date="test" value="Submit" />
          </form>
          <span>{this.state.formattedDate}</span>
      </div>
      )
  }

  handleChange(event) {
    this.setState({value: event.target.value});
  }

  handleSubmit(event) {
    event.preventDefault();
    setUserSteps(this.state.value, this.state.date);
  }

}

export default UserStep;