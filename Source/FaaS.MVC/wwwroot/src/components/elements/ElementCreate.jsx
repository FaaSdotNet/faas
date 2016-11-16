import React, { Component } from "react";
import MyInput from "../form/MyInput"
import MySubmit from "../form/MySubmit"


export class ElementCreateComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        const elementDescription = this.refs.elementDescription.state.value;
        const elementOptions = this.refs.elementOptions.state.value;
        const elementType = this.refs.elementType.state.value;
        const elementRequired = this.refs.elementRequired.state.value;

        var result = fetch('/api/v1.0/elements', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: "same-origin",
            body: JSON.stringify({
                Description: elementDescription,
                Options : elementOptions,
                Type : elementType,
                Required : elementRequired
            })
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/elements";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <MyInput ref="elementDescription" id="elementDescription" label="Description"/>
                <MyInput ref="elementOptions" id="elementOptions" label="Options"/>
                <MyInput ref="elementType" id="elementType" label="Type"/>
                <MyInput ref="elementRequired" id="elementRequired" label="Required"/>
                
                <MySubmit ref="elementSubmit" onClick={this.handleSubmit} id="elementButton" value="Create"/>
            </div>
        );
    }
}

export default ElementCreateComponent;