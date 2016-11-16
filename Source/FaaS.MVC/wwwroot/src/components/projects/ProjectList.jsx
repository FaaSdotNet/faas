import React, { Component } from "react";
import MyViewTable from "../table/MyViewTable"


export class ProjectListComponent extends Component {

    constructor(props) {
        super(props);
    }

    handleAdd(event) {
        document.location.href = "/#/projects/create/";
    }

    render() {
        return (
            <div>
                <div className="row">
                    <MyViewTable name="projects" propName="projectName" />
                </div>
                <button onClick={() => {this.handleAdd()}}
                            type="button" className="btn btn-primary btn-md" role="button">Add New Project</button>
            </div>
        );
    }
}

export default ProjectListComponent;