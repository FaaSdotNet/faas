import React, { Component } from "react";
import MyInput from "../form/MyInput"
import MySubmit from "../form/MySubmit"


export class FormCreateComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        const formName = this.refs.formName.state.value;
        const formDesc = this.refs.formDescription.state.value;

        var result = fetch('/api/v1.0/forms', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: "same-origin",
            body: JSON.stringify({
                FormName: formName,
                Description: formDesc
            })
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/forms";
                    });
            }
        });
    }

    handleCancel(event) {
        document.location.href = "/#/forms";
    }

    render() {
        return (
            <div className="form-horizontal">
                <MyInput ref="formName" id="formName" label="Form Name"/>
                <MyInput ref="formDescription" id="formDescription" label="Description"/>
                
                <MySubmit ref="formSubmit" onClick={this.handleSubmit} id="formButton" value="Create"/>
                <MySubmit ref="formCancel" onClick={this.handleCancel} id="formCancel" value="Cancel"/>
            </div>
        );
    }
}

export default FormCreateComponent;