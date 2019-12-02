import React, { Component } from 'react';
import { Route } from 'react-router';
import { AdminLayout } from './components/AdminLayout';
import Auth from './components/Auth';
import { EditParticipant } from './components/admin/editParticipant';
import { EditTeams } from './components/admin/editTeams';
import { EditChallengeSettings } from './components/admin/editChallengeSettings';
import { AdminScoreBoard } from './components/admin/adminScoreBoard';
import { TotalOverview } from './components/admin/totalOverview';

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
    this.auth = new Auth()
    this.auth.isAdminRequest()
        .then(isAdmin => {
            if(!isAdmin){
              console.log("not an admin")
              // redirect to home if not an admin
              window.location.href = "/";
            }else{
              this.setState({
                  loading: false,
              })
            }
        })
  }

  render () {
      return (
        <div>
          {this.state.loading &&
            <p><em>Loading...</em></p>
          }
          {!this.state.loading &&
            <div>
              <AdminLayout >
                <Route exact path='/admin/' component={TotalOverview} />
                <Route path='/admin/participants' component={EditParticipant} />
                <Route path='/admin/teams' component={EditTeams} />
                <Route path='/admin/settings' component={EditChallengeSettings} />
                <Route path='/admin/leaderboard' component={AdminScoreBoard} />
              </AdminLayout>
            </div>
          }
          </div>
      );
  }
}