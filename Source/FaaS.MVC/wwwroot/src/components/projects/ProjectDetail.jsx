import React, { Component } from "react";
import FormList from "../forms/FormList"

export class ProjectDetail extends Component {

    constructor(props) {
        super(props);

        this.state = null;
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
            if(res.ok) {
                res.json()
                    .then((js) => {
                        this.setState(js);
                    });
            }
        });
    }

    render() {
        var rows =[];
        var state = this.state;
        if (state) {
            rows.push(<h2 key={state.projectName}><b>Project: </b>{state.projectName}</h2>);
            rows.push(<p key={state.description}>{state.description}</p>);
            rows.push(<p key={state.created}><b>Created: </b>{state.created}</p>);
        }

        return (
            <div>
                {rows}
                <FormList />
            </div>
       );
    }
}

export default ProjectDetail;