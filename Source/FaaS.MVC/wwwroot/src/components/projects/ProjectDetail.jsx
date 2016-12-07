import React, { Component } from "react";
import {connect} from "react-redux";


@connect((store) => {
   return store;
})
export class ProjectDetail extends Component {

    constructor(props) {
        super(props);
        this.projectId = this.props.projectId;

        this.state = {};
    }

    componentWillMount() {
        const result = fetch(`/api/v1.0/projects/${this.projectId}`,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': "application/json"
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

    render() {
        const state = this.state;
        return (
            <div>
                <div>
                    <h1>
                        <strong>Project:</strong> {state.projectName}
                    </h1>
                    <pre>
                        {state.description}
                        <p><emph>(Created: {state.created})</emph></p>
                    </pre>

                </div>
            </div>
       );
    }
}

export default ProjectDetail;