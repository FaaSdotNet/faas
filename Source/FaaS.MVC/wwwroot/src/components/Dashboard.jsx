import React, { Component } from 'react';
import ProjectList from './projects/ProjectList'
import {connect} from "react-redux";

import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';



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

        let dashboard_content =
            <div>
                <h3>Projects</h3>
                <ProjectList projects={this.props.projects} />;
            </div>;
        if(this.props.page.project != null){
            dashboard_content =
                <div>
                    <h3>Project {this.props.pages.project.projectName}</h3>
                    <FormsList forms={this.props.projects} />;
                </div>;
        }
        if(this.props.page.form != null) {
			dashboard_content =
                <div>
                    <h3>Form {this.props.pages.project.projectName} </h3>
                    <FormsList forms={this.props.projects} />;
                </div>;
        }

        return (
        <div className="col-md-12 col-xs-12 row">
            {dashboard_content}
          </div>
      );
    }
}

export default Dashboard;