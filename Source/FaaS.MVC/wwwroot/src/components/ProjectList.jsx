import React, { Component } from "react";
import MyViewTable from "./table/MyViewTable"


export class ProjectListComponent extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="row">
                <div className="row">
                    <a className="btn btn-info btn-md" role="button" href="/#/projects/create/"> Add project</a>
                </div>
            <MyViewTable name="projects" propName="projectName" />
            </div>
        );
    }
}

export default ProjectListComponent;