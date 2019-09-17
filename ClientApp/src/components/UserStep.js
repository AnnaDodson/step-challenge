import React, { Component } from 'react'
import * as moment from 'moment';

var userId;
var teamId = 1;

async function setUserSteps(newValue, date){
    var dateISO = moment(date).format();
    console.log(dateISO);
  const response = await fetch('/graphql', {
     method:'POST',
     headers:{'content-type':'application/json'},
     body:JSON.stringify({query:
      `mutation creatStepsEntry {creatStepsEntry ( steps: { stepCount : ${newValue} , dateOfSteps : "${dateISO}", userId : ${userId} }){stepCount}}`
    })
  });
  const responseBody = await response.json();
  var result = null;
  if(responseBody.hasOwnProperty("creatStepsEntry") && responseBody.creatStepsEntry.hasOwnProperty("stepCount") ){
      result = responseBody.creatStepsEntry.stepCount;
  }
  return result;
}

class UserStep extends Component {
  constructor(props) {
      console.log("here");
    super(props);
    userId = props.data.userId;
    var formatDate = moment(props.data.date).format('MMM Do');
    this.state =  {
       step: props.data,
       steps: props.data.count,
       formattedDate: formatDate,
       date: props.data.date,
       value : props.data.count,
       editing: false
    }
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleEditing = this.handleEditing.bind(this);
    this.setSteps = this.setSteps.bind(this);
  }

  render() {
      return(
        <div>
          <p>{this.state.steps}</p>
        {!this.state.editing &&
          <button type="button" className="btn btn-info" onClick={this.handleEditing}>Edit</button>
        }
        {this.state.editing &&
            <div>
              <form onSubmit={this.handleSubmit} key={this.state.steps}>
                <label>
                  <input type="text" value={this.state.value} onChange={this.handleChange} />
                </label>
                <br />
                <input type="submit" data-date="test" value="Submit" />
              </form>
            </div>
        }
        <p>{this.state.formattedDate}</p>
      </div>
      )
  }

  handleEditing(event){
      this.setState({editing: true})
  }

  handleChange(event) {
    this.setState({value: event.target.value});
  }

  handleSubmit(event) {
      var self = this;
    event.preventDefault();
    setUserSteps(this.state.value, this.state.date).then(function(res){
        if(res != null){
            self.setSteps(res);
        } //todo add validation here
    })
  }

  setSteps(updatedValue){
    this.setState({
        steps : updatedValue,
        editing: false,
    })
  }

}

export default UserStep;