import React, { Component } from 'react'

class Team extends Component {
  render() {
    return (
      <div>
        <div>
          {this.props.team.name}
        </div>
      </div>
    )
  }
}

export default Team