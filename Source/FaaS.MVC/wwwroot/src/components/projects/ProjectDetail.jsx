import React, { Component } from "react";
import FormList from "../forms/FormList"
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
            if(res.ok) {
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
                <table className="table table-condensed">
                    <tr>
                        <td>
                            <strong>Project</strong>
                        </td>
                        <td>
							{state.projectName}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Description</strong>
                        </td>
                        <td>
							{state.description}
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Created</strong>
                        </td>
                        <td>
							{state.created}
                        </td>
                    </tr>
                </table>
            </div>
       );
    }
}

export default ProjectDetail;