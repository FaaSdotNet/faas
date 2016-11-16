import React, { Component } from "react";
import ElementList from "../elements/ElementList"

export class FormDetail extends Component {

    constructor(props) {
        super(props);

        this.state = null;
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/forms/' + this.props.params.formid,
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
            rows.push(<p key={state.formName}><b>Form Name: </b>{state.formName}</p>);
            rows.push(<p key={state.description}><b>Description: </b>{state.description}</p>);
            rows.push(<p key={state.created}><b>Created: </b>{state.created}</p>);
        }

        return (
            <div>
                <h2>Form Details</h2>
                {rows}
                <ElementList />
            </div>
       );
    }
}

export default FormDetail;