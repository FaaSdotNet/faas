﻿import React, { Component } from "react";
import MyInput from "./form/MyInput"
import MySubmit from "./form/MySubmit"


export class ProjectCreateComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    handleSubmit(event) {
        const projectName = this.refs.projectName.state.value;
        const projectDesc = this.refs.projectDescription.state.value;
        console.log('Creating new project: ', projectName);
        var result = fetch('/api/v1.0/projects', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: "same-origin",
            body: JSON.stringify({
                ProjectName: projectName,
                Description: projectDesc
            })
        });

        result.then( (res) =>  {
            console.log(res);
                
            if (res.ok) {

                res.json()
                    .then((js) => {
                        console.log(js);
                        console.info("project id: ", js.id);
                        document.location.href ="/#/projects";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <MyInput ref="projectName" id="projectName" label="Project Name"/>
                <MyInput ref="projectDescription" id="projectDescription" label="Description"/>
                
                <MySubmit ref="projectSubmit" onClick={this.handleSubmit} id="projectButton" value="Create"/>
            </div>
        );
    }
}

export default ProjectCreateComponent;