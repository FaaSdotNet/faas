import React, { Component } from "react";


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
            rows.push(<p key={state.projectName}><b>Project Name: </b>{state.projectName}</p>);
            rows.push(<p key={state.description}><b>Description: </b>{state.description}</p>);
            rows.push(<p key={state.created}><b>Created: </b>{state.created}</p>);
        }

        return (
            <div>
                <h2>Project Details</h2>
                {rows}
            </div>
       );
    }
}

export default ProjectDetail;