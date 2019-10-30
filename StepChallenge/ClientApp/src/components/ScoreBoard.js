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
        const numberOfStepsPerMile = 2000;
        var totalMiles = totalSteps * (1 / numberOfStepsPerMile);
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
                        <td colSpan="2" width="100%">{score.teamName}</td>
                    </tr>
                );

            var total = (
                <tr>
                    <td width="50%">Total</td>
                    <td width="50%">{totalSteps} steps ({totalMiles} miles)</td>
                </tr>
            );
            return(
                <div>
                    <h3>Updated every Monday<span style={{ fontStyle: "italic", fontSize: "16px" }}> Any new steps added for previous weeks will be included</span> </h3>
                    <table className='table table-striped' style={{ textAlign : "center", fontSize: "24px" }} >
                        <thead>
                            <tr>
                                <th colSpan="2" >Teams Step Counts until { dateOfScores }</th>
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
