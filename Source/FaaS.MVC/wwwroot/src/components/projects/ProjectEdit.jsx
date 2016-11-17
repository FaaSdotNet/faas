import React, { Component } from "react";

class ProjectEdit extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleProjectNameChange = this.handleProjectNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);

        this.state = {};
        this.state.projectName = "";
        this.state.description = "";
        this.state.created = "";
    }

    handleProjectNameChange(event) {
        this.setState({projectName: event.target.value,
            description: this.state.description,
            created: this.state.created});
    }

    handleDescriptionChange(event) {
        this.setState({projectName: this.state.projectName,
            description: event.target.value,
            created: this.state.created});
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/projects/' + this.props.params.projectid,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json'
            }
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        const projectName = this.state.projectName;
        const description = this.state.description;
        var result = fetch('/api/v1.0/projects', {
            method: 'PUT',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: this.props.params.projectid,
                projectName: projectName,
                Description: description,
                Created: this.state.created
            })
        });
        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/projects";
                    });
            }
        });
    }
    
    handleCancel(event) {
        document.location.href = "/#/projects"
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>Edit Project</h4>
                <label htmlFor="projectName" className="col-md-5 control-label">
                    Project Name
                </label>
                <input ref="editProjectName" type="text" id="ProjectName"
                       onChange={this.handleProjectNameChange} className="form-control"
                       value={this.state.projectName} />
                <label htmlFor="description" className="col-md-5 control-label">
                    Description
                </label>
                <input ref="editDescription" type="text" id="Description"
                       onChange={this.handleDescriptionChange} className="form-control"
                       value={this.state.description} />

                <br/>
                <input type="button" 
                        id="editButton"
                        onClick={this.handleSubmit}
                        value="Save" 
                        className="btn btn-primary col-md-offset-5"/>

                <input type="button" 
                        id="cancelButton"
                        onClick={this.handleCancel}
                        value="Cancel" 
                        className="btn btn-default"/>
            </div>
        );
    }
}

export default ProjectEdit;