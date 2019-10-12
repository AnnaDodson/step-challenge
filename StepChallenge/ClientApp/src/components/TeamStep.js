import React, { Component } from 'react'
import * as moment from 'moment';
import { stat } from 'fs';

const infoStyle = {
  "width": (100/6) + "%" ,
  "height": "10px",
  "background-color" : "#dee2e6",
  "border": "1px solid #b1b4b8",
  "display": "inline-block"
}

const infoMissing = {
  "width": (100/6) + "%" ,
  "height": "10px",
  "background-color" : "#dee2e6",
  "border": "1px solid #c8cacd",
  "display": "inline-block"
}

const infoDone = {
  "width": (100/6) + "%" ,
  "height": "10px",
  "border": "1px solid #c8cacd",
  "display": "inline-block",
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
        steps: false
      }
      if(this.state.participantsStatus[i] != null ){
        status.name = this.state.participantsStatus[i].participantName
        status.steps = this.state.participantsStatus[i].participantAddedStepCount
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
            <span role="button" title={s.name} style={ s.steps ? infoDone : infoMissing }></span>
          )}
        </div>
      </div>
    )
  }
}


export default TeamStep;