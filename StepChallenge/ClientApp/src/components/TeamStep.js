import React, { Component } from 'react'
import * as moment from 'moment';

const infoMissing = {
  "background-color" : "#dee2e6",
  "border": "1px solid #c8cacd",
}

const infoDone = {
  "border": "1px solid #c8cacd",
  "background-color" : "#66db79",
}

class TeamStep extends Component {
  constructor(props) {
    super(props);
    var formatDate = moment(props.data.date).format('MMM Do');
    this.state =  {
       step: props.data,
       steps: props.data.count,
       formattedDate: formatDate,
       participantsStatus: props.data.participantsStatus ? props.data.participantsStatus : [],
       numberOfParticipants : props.data.numberOfParticipants,
       date: props.data.date,
       value : props.data.count,
       editing: false
    }
  }

  render() {
    var info = []
    for (var i = 0; i < this.state.numberOfParticipants; i++) {
      var status = {
        name : "User not registered yet",
        initials: "-",
        steps: false,
        highestStepper: false
      }
      if(this.state.participantsStatus[i] != null ){
        status.name = this.state.participantsStatus[i].participantName
        status.initials = this.state.participantsStatus[i].participantName.charAt(0).toUpperCase()
        status.steps = this.state.participantsStatus[i].participantAddedStepCount
        status.highestStepper = this.state.participantsStatus[i].participantHighestStepper
      }
        info[i] = status
    }
    return (
        <div style={{"width":"100%"}}>
        <span>{this.state.steps}</span>
        <br/>
        <span>{this.state.formattedDate}</span>
        <br/>
         <div style={{"width":"100%"}}>
          {info.map(s =>
            <span title={this.state.steps != 0 ? s.name + (s.highestStepper ? " - well done on being top stepper!" : "") : "no steps counted"} className={s.highestStepper == true ? "goldStar statusInfo" : "statusInfo"} style={  s.steps ? infoDone : this.state.steps != 0 ? infoMissing : null}>{s.initials}</span>
          )}
        </div>
      </div>
    )
  }
}

export default TeamStep;