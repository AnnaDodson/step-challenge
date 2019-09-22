import React, { Component } from 'react'
import * as moment from 'moment';
import { BrowserIcon, ICON } from '../icons/EditIcon';

async function setUserSteps(newValue, date){
  // TODO - just take the date and remove the time plus any time zone stuff
  // Moment and GraphQL don't like dates
  var format = moment(date);
  var dateString = format.year() + "-" + ( format.month() + 1 ) + "-" + format.date() + "T00:00:00+00:00";
  const response = await fetch('/graphql', {
   method:'POST',
   headers:{'content-type':'application/json'},
   body: JSON.stringify({
       "query": ` mutation creatStepsEntry ( $participantId : Int! )
       { creatStepsEntry ( steps: {  	stepCount : ${newValue} , dateOfSteps :  "${dateString}"  , participantId :  $participantId } )  {stepCount} } `
       , "variables" : { "participantId" : 1 }  
      })
  })
  const responseBody = await response.json();
  var result = null;
  if(responseBody.hasOwnProperty("creatStepsEntry") && responseBody.creatStepsEntry.hasOwnProperty("stepCount") ){
      result = responseBody.creatStepsEntry.stepCount;
  }
  return result;
}

const inputStyle = {
  width: "20%"
};

class UserStep extends Component {
  constructor(props) {
    super(props);
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
          <p>{this.state.formattedDate}</p>
          {!this.state.editing &&
            <p>{this.state.steps}
              <button type="button" className="btn " onClick={this.handleEditing}>
                <BrowserIcon color={ICON.COLORS.BRAND} size={ICON.SIZES.MED} />
              </button>
            </p>
          }
          {this.state.editing &&
              <div>
                <form style={inputStyle} onSubmit={this.handleSubmit} key={this.state.steps}>
                  <label>
                    <input type="text" value={this.state.value} onChange={this.handleChange} />
                  </label>
                  <br />
                  <input type="submit" data-date="test" value="Submit" />
                </form>
              </div>
          }
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