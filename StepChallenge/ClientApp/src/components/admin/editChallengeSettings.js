import React, { Component } from 'react';
import ApiHelper from '../ApiHelper';

async function updateSetting(settings){
  const response = await fetch('/graphql', {
   method:'POST',
   headers:{'content-type':'application/json'},
   body: JSON.stringify({
       "query": ` mutation updateChallengeSettings { updateChallengeSettings ( settings : {  showLeaderBoard : ${settings.showLeaderBoard}, showLeaderBoardStepCounts : ${settings.displayTeamScores} } )  { showLeaderBoard, showLeaderBoardStepCounts  } } `
      })
  })
  const responseBody = await response.json();
  var result = null;
  if(responseBody.hasOwnProperty("updateChallengeSettings")){
      result = responseBody.updateChallengeSettings;
  }
  else{
      result = { error : "Failed to save the change" }
  }
  return result;
}

export class EditChallengeSettings extends Component {
  static displayName = EditChallengeSettings.name;

  constructor (props) {
    super(props);
    this.state = {
        loading: true,
        showLeaderBoard : false,
        displayTeamScores : false,
        error : false
    }
    this.handleSaveDisplayLeaderBoard = this.handleSaveDisplayLeaderBoard.bind(this);
    this.handleSaveDisplayTeamScores = this.handleSaveDisplayTeamScores.bind(this);
    this.saveNewSettings = this.saveNewSettings.bind(this);
    var query = `{ "query": "{ challengeSettings { showLeaderBoard, showLeaderBoardStepCounts } }" }`
    this.apiHelper = new ApiHelper();
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
        this.setState({
            showLeaderBoard: data.challengeSettings.showLeaderBoard,
            displayTeamScores: data.challengeSettings.showLeaderBoardStepCounts,
            loading: false
        });
    })
  }

  handleSaveDisplayLeaderBoard(setting) {
    var newState = this.state.showLeaderBoard == true ? false : true;
    this.setState({showLeaderBoard : newState, error: false})
    updateSetting({
        showLeaderBoard : newState,
        displayTeamScores : this.state.displayTeamScores
    }).then(data =>{
      if(data.hasOwnProperty("showLeaderBoard")){
        this.setState({
            showLeaderBoard : data.showLeaderBoard,
            displayTeamScores : data.showLeaderBoardStepCounts
        })
      }else{
        this.setState({
            error : true
        })
      }
    })
  }

  handleSaveDisplayTeamScores(setting) {
    var newSettings = this.state.displayTeamScores == true ? false : true;
    this.setState({displayTeamScores : newSettings, error : false})
    updateSetting({
        showLeaderBoard : this.state.showLeaderBoard,
        displayTeamScores : newSettings
    }).then(data =>{
      if(data.hasOwnProperty("showLeaderBoard")){
        this.setState({
            showLeaderBoard : data.showLeaderBoard,
            displayTeamScores : data.showLeaderBoardStepCounts
        })
      }else{
        this.setState({
            error : true
        })
      }
    })
  }

  saveNewSettings(){
    updateSetting({
        showLeaderBoard : this.state.showLeaderBoard,
        displayTeamScores : this.state.displayTeamScores
    }).then(data =>{
        this.setState({
            showLeaderBoard : data.showLeaderBoard,
            displayTeamScores : data.showLeaderBoardStepCounts
        })
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
                <div>
                    <label>
                    Show leader Board
                    </label>
                    <input type="checkbox" name="showLeaderBoard" key={"showLeaderBoard"} checked={this.state.showLeaderBoard} style={{ marginLeft: "8px"}} onChange={this.handleSaveDisplayLeaderBoard} />
                    <br />
                </div>
                <div>
                    <br />
                    <label>
                    Show Step Counts on Leader Board
                    </label>
                    <input type="checkbox" name="showLeaderBoard" key={"displayTeamScores"} checked={this.state.displayTeamScores} style={{ marginLeft: "8px"}} onChange={this.handleSaveDisplayTeamScores} />
                </div>
                {this.state.error &&
                  <div>
                    <br />
                      <p style={{"color": "red"}}>Failed to save. Try again later</p>
                  </div>
                }
            </div>
          }
        </div>
    );
}}
