import React, { Component } from 'react';
import ProjectList from './projects/ProjectList'

class Dashboard extends Component {

    constructor(props) {
        super(props);
        const user = this.props.user;

        if (user == null) {
             document.location.href = "/#/index";
         }
    }

    render() {
        return (
        <div className="col-md-12 col-xs-12 row">
                <h3>Your Projects</h3>
            <ProjectList  />
          </div>
      );
    }
}

export default Dashboard;