import React, { Component } from "react";
import {ProjectListTable} from "./ProjectListTable"


export class ProjectListComponent extends Component {

    constructor(props) {
        super(props);
    }

    static handleAdd(event) {
        document.location.href = "/#/projects/create/";
    }

    render() {
        return (
            <div>
                <div className="row">
                    <ProjectListTable  />
                </div>
                <button onClick={() => {this.handleAdd()}}
                            type="button" className="btn btn-primary btn-md" role="button">Add New Project</button>
            </div>
        );
    }
}

export default ProjectListComponent;