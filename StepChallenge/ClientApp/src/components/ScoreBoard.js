import React, { Component } from 'react';
import ApiHelper from './ApiHelper';
import * as moment from 'moment';


export class ScoreBoard extends Component {
    static displayName = ScoreBoard.name;

    constructor (props) {
        super(props);
        this.state = {
            scores: [],
            totalSteps : 0,
            dateOfLeaderboard : null,
            loading: true,
            showLeaderBoardStepCounts : false,
            showLeaderBoard : false,
            error : false
        }
        this.apiHelper = new ApiHelper();
        var query = `{ "query": "{ leaderBoard
       { dateOfLeaderboard, teamScores { teamId, teamName, teamStepCount }, totalSteps},
        challengeSettings { showLeaderBoard, showLeaderBoardStepCounts } }"
      }`
        this.apiHelper.GraphQlApiHelper(query)
            .then(data => {
                if(data.hasOwnProperty("leaderBoard")){
                    this.setState({
                        dateOfLeaderboard: data.leaderBoard.dateOfLeaderboard,
                        scores: data.leaderBoard.teamScores,
                        totalSteps: data.leaderBoard.totalSteps,
                        showLeaderBoard: data.challengeSettings.showLeaderBoard,
                        showLeaderBoardStepCounts: data.challengeSettings.showLeaderBoardStepCounts,
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

    static renderScoreBoard (date, scores, totalSteps, displayLeaderBoard, showLeaderBoardStepCounts, error) {
        var dateOfScores = moment(date).format('Do MMM')
        if(displayLeaderBoard && !error){
            var teams = showLeaderBoardStepCounts ?
                scores.map(score =>
                    <tr key={score.teamId}>
                        <td width="50%">{score.teamName}</td>
                        <td width="50%">{score.teamStepCount}</td>
                    </tr>
                )
                :
                scores.map(score =>
                    <tr key={score.teamId}>
                        <td width="100%">{score.teamName}</td>
                    </tr>
                );
            var colSpan = showLeaderBoardStepCounts ? 2 : 1;
            var total = showLeaderBoardStepCounts ? (
                <tr>
                    <td width="50%">Total steps</td>
                    <td width="50%">{totalSteps}</td>
                </tr>
            ) : null;
            return(
                <div>
                    <h3>Updated every Monday<span style={{ fontStyle: "italic", fontSize: "16px" }}> Any new steps added for previous weeks will be included</span> </h3>
                    <table className='table table-striped' style={{ textAlign : "center", fontSize: "24px" }} >
                        <thead>
                            <tr>
                                <th colSpan={colSpan} >Teams Step Counts until { dateOfScores }</th>
                            </tr>
                        </thead>
                        <tbody>
                            { teams }
                            { total }
                        </tbody>
                    </table>
                </div>
            );
        }
        else{
            var message = error ?
                (
                    <div>
                        <h4>There's been a terrible misunderstanding. Try again later</h4>
                    </div>
                ) :
                (
                    <div>
                        <h3>coming soon</h3>
                    </div>
                );
            return message
        }
    }

    render () {
        let contents =
            this.state.loading
                ? <p><em>Loading...</em></p>
                : ScoreBoard.renderScoreBoard(this.state.dateOfLeaderboard, this.state.scores, this.state.totalSteps, this.state.showLeaderBoard, this.state.showLeaderBoardStepCounts, this.state.error);

        return (
            <div>
                <h2>Leader Board</h2>
                {contents}
            </div>
        );
    }
}
