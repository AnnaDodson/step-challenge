import React, { Component } from 'react';
import ApiHelper from './ApiHelper';
import * as moment from 'moment';

export class ScoreBoard extends Component {
  static displayName = ScoreBoard.name;

  constructor (props) {
    super(props);
    this.state = {
      scores: [],
      dateOfLeaderboard : null,
      loading: true
    };
    this.apiHelper = new ApiHelper();
    var query = `{"query" : "{ leaderBoard{ dateOfLeaderboard, teamScores { teamId, teamName } }}"}`
    this.apiHelper.GraphQlApiHelper(query)
    .then(data => {
        this.setState({ dateOfLeaderboard: data.leaderBoard.dateOfLeaderboard, scores: data.leaderBoard.teamScores, loading: false });
    })
  }

  static renderScoreBoard (date, scores) {
    var dateOfScores = moment(date).format('Do MMM')
      return(
      <div>
        <h3>Updated every Monday<span style={{ fontStyle: "italic", fontSize: "16px" }}> Any new steps added for previous weeks will be included</span> </h3>
        <table className='table table-striped' style={{ textAlign : "center", fontSize: "24px" }} >
          <thead>
            <tr>
              <th>Teams Step Counts until { dateOfScores }</th>
            </tr>
          </thead>
          <tbody>
            {scores.map(score =>
              <tr key={score.teamId}>
                <td>{score.teamName}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }

  render () {
    let contents =
    this.state.loading
      ? <p><em>Loading...</em></p>
      : ScoreBoard.renderScoreBoard(this.state.dateOfLeaderboard, this.state.scores);

    return (
      <div>
        <h2>Leader Board</h2>
        {contents}
      </div>
    );
  }
}
