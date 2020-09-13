import React from 'react';
import { Link } from 'react-router-dom';

class Information extends React.Component {
  render() {
    return (
        <div className="col-md-4">
            <h1>{this.props.puppy.alias}</h1>
            <p>Country: {this.props.puppy.localization.country}</p>
            <p>City: {this.props.puppy.localization.city}</p>
            <p>Street: {this.props.puppy.localization.street}</p>
        </div>
    );
  }
}

export default Information;
