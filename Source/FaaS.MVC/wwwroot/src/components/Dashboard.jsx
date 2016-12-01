import React, { Component } from 'react';
import ProjectList from './projects/ProjectList'
import {connect} from "react-redux";


@connect((store) => {
	return store;
})
class Dashboard extends Component {

    constructor(props) {
        super(props);
        const user = this.props.user.isAuthenticated;
        console.log( "Dasboard:", this.props);

        if (!user) {
             //document.location.href = "/#/index";
         }
    }

    render() {
        return (
        <div className="col-md-12 col-xs-12 row">
                <h3>Your Projects</h3>
            <ProjectList projects={this.props.projects} />
          </div>
      );
    }
}

export default Dashboard;