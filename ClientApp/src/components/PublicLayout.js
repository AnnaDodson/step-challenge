import React, { Component } from 'react';
import { Container } from 'reactstrap';

export class PublicLayout extends Component {
  static displayName = PublicLayout.name;

  render () {
    return (
      <div>
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}
