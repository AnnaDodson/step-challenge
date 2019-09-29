import React, { Component } from 'react';
import ApiHelper from './ApiHelper';

async function loadUsers() {
 var query = `{ "query": "query usersQuery { participants { isAdmin, participantName, participantId, team {teamName} } }" }`
  var apiHelper = new ApiHelper();
  const response = await apiHelper.GraphQlApiHelper(query);
  return response.participants;
}

export class Admin extends Component {
  static displayName = Admin.name;

  constructor(props) {
    super(props);
    this.state =  {
        users : []
    }
  }

  //componentDidUpdate
  componentDidMount() {
    loadUsers().then(res =>
      this.setState({
        users : res != null ? res: [],
        loading: false,
      })
    )
  }

  static renderUsersList (users) {
    return (
        <div>
            <ul>
          {users.map(user =>
            <li>{user.participantName}, </li>
          )}
          </ul>
        </div>

        )
  }

  render () {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Admin.renderUsersList(this.state.users);

    return (
        <div>
            {contents}
        </div>
    );
  }
}