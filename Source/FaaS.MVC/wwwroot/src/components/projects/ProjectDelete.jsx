import React, { Component } from "react";

class ProjectDelete extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);

        this.state = {};
        this.state.projectName = "";
        this.state.description = "";
        this.state.created = "";
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
        var result = fetch('/api/v1.0/projects/' + this.props.params.projectid,
        {
            method: 'DELETE',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/login";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>Delete Project</h4>
                <p>Delete project with Name: {this.state.projectName} ?</p>

                <input type="button" 
                    id="deleteButton"
                    onClick={this.handleSubmit}
                    value="Delete" 
                    className="btn btn-default"/>
            </div>
        );
    }
}

export default ProjectDelete;