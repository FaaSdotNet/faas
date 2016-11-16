import React, { Component } from "react";
import MyInput from "./form/MyInput"
import MySubmit from "./form/MySubmit"

class RegisterComponent extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        const googleId = this.refs.registerGoogleId.state.value;
        const userName = this.refs.registerUserName.state.value;

        var result = fetch('/api/v1.0/users', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                GoogleId: googleId,
                UserName: userName
            })
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
                <h4>
                    Sign up with your Google Account
                </h4>
                <MyInput ref="registerGoogleId" id="GoogleId" label="Google ID" />
                <MyInput ref="registerUserName" id="UserName" label="Username" />

                <MySubmit ref="registerButton" onClick={this.handleSubmit} id="registerButton" value="Sign Up"/>


            </div>
        );
    }
}

export default RegisterComponent;