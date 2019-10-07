import React, { Component } from 'react';
import { EditParticipant } from './admin/editParticipant';
import { EditTeams } from './admin/editTeams';
import { EditChallengeSettings } from './admin/editChallengeSettings';
import { AdminScoreBoard } from './admin/adminScoreBoard';

const views = {
  editParticipant : 1,
  editChallengeSettings : 2,
  editTeams : 3,
  showLeaderBoard : 4,
}

export class Admin extends Component {
  static displayName = Admin.name;

  constructor(props) {
    super(props);
    this.state =  {
        loading: true,
        editParticipant : false,
        editTeams : false,
        editChallengeSettings : false,
        showLeaderBoard : false,
    }
    this.setView = this.setView.bind(this)
  }

  setView(view) {
    this.setState({editChallengeSettings: false})
    this.setState({editParticipant: false})
    this.setState({editTeams: false})
    this.setState({showLeaderBoard: false})
    switch(view){
      case views.editParticipant:
        this.setState({editParticipant: true})
        break;
      case views.editTeams:
        this.setState({editTeams: true})
        break;
      case views.editChallengeSettings:
        this.setState({editChallengeSettings: true})
        break;
      case views.showLeaderBoard:
        this.setState({showLeaderBoard: true})
        break;
      default:
        this.setState({editParticipant: true})
        break;
    }
  }

  render () {
    return (
        <div>
          <p>
            <button className="btn btn-link" key={'editParticipant'} onClick={() => this.setView(views.editParticipant)}>Edit Participants</button>
            <button className="btn btn-link" key={'editTeams'} onClick={() => this.setView(views.editTeams)}>Edit Teams</button>
            <button className="btn btn-link" key={'editChallengeSettings'} onClick={() => this.setView(views.editChallengeSettings)}>Edit Settings</button>
            <button className="btn btn-link" key={'showLeaderBoard'} onClick={() => this.setView(views.showLeaderBoard)}>See Leader Board</button>
          </p>

          {this.state.editChallengeSettings &&
            <EditChallengeSettings />
          }
          {this.state.editParticipant &&
            <EditParticipant />
          }
          {this.state.editTeams &&
            <EditTeams />
          }
          {this.state.showLeaderBoard &&
            <AdminScoreBoard />
          }
        </div>
    );
  }
}