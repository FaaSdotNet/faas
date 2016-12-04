import React, { Component } from "react";
import FormList from "../forms/FormList"
import {connect} from "react-redux";


@connect((store) => {
   return store;
})
export class ProjectDetail extends Component {

    constructor(props) {
        super(props);
        this.projectid = this.props.params.projectid;

        this.state = null;
    }

    componentWillMount() {
        const result = fetch(`/api/v1.0/projects/${this.projectid}`,
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
        let rows =[];
        const state = this.state;
        if (state) {
            rows.push(<h2 key={state.projectName}><b>Project: </b> {state.projectName} </h2>);
            rows.push(<div key={state.description}>{state.description}</div>);
            rows.push(<p key={state.created}><b>Created: </b>{state.created}</p>);
        }

        return (
            <div>
                {rows}
                <FormList projectid={this.projectid} />
            </div>
       );
    }
}

export default ProjectDetail;