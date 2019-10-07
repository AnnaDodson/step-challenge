import React, { Component } from 'react';
import ApiHelper from './../ApiHelper';
import * as moment from 'moment';


export class AdminScoreBoard extends Component {
  static displayName = AdminScoreBoard.name;

  constructor (props) {
    super(props);
    this.state = {
      scores: [],
      dateOfLeaderboard : null,
      loading: true,
      showLeaderBoardStepCounts : false,
      error : false
    }
    this.apiHelper = new ApiHelper();
    var query = `{ "query": "{ adminLeaderBoard
       { dateOfLeaderboard, teamScores { teamId, teamName, teamStepCount }} }" }`
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
      if(data.hasOwnProperty("adminLeaderBoard")){
        this.setState({
          dateOfLeaderboard: data.adminLeaderBoard.dateOfLeaderboard,
          scores: data.adminLeaderBoard.teamScores,
          loading: false
        });
      }else{
        this.setState({
          loading: false,
          error : true
        });
      }
    })
  }

  static renderScoreBoard (date, scores, error) {
    var dateOfScores = moment(date).format('Do MMM')
    if(!error){
        return(
        <div>
          <h4>Updated every Monday<span style={{ fontStyle: "italic", fontSize: "16px" }}> Any new steps added for previous weeks will be included</span> </h4>
          <table className='table table-striped' style={{ textAlign : "center", fontSize: "24px" }} >
            <thead>
              <tr>
                <th colSpan="2" >Teams Step Counts until { dateOfScores }</th>
              </tr>
            </thead>
            <tbody>
              {scores.map(score =>
                <tr key={score.teamId}>
                  <td width="50%">{score.teamName}</td>
                  <td width="50%">{score.teamStepCount}</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      );
    }
    else{
      return (
          <div>
            <h4>There's been a terrible misunderstanding. Try again later</h4>
          </div>
        )
    }
  }

  render () {
    let contents =
    this.state.loading
      ? <p><em>Loading...</em></p>
      : AdminScoreBoard.renderScoreBoard(this.state.dateOfLeaderboard, this.state.scores, this.state.error);

    return (
      <div>
        {contents}
      </div>
    );
  }
}
