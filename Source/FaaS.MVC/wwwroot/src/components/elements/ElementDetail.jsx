import React, { Component } from "react";

export class ElementDetail extends Component {

    constructor(props) {
        super(props);

        this.state = null;
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/elements/' + this.props.params.elementid,
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
        var rows = [];
        var state = this.state;
        if (state) {
            rows.push(<p key={state.description}><b>Description: </b>{state.description}</p>);
            rows.push(<p key={state.options}><b>Options: </b>{state.options}</p>);
            rows.push(<p key={state.type}><b>Type: </b>{state.type}</p>);
            rows.push(<p key={state.required}><b>Required: </b>{state.required}</p>);
        }

        return (
            <div>
                <h2>Element Details</h2>
                {rows}
            </div>
       );
    }
}

export default ElementDetail;