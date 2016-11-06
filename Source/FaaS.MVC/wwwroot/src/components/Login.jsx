import React, { Component } from "react";
import MyInput from "./form/MyInput"
import MySubmit from "./form/MySubmit"


export class LoginComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }


    handleSubmit(event) {
        const googleId = this.refs.loginGoogleId.state.value;
        console.log('Logging in as: ', googleId);
        var result = fetch('/api/v1.0/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    GoogleId: googleId,
                })
            });
            result.then( (res) =>  {
                console.log(res);
                console.log(res.headers);
                res.json().then((js) => console.log(js));
                console.log(" Cookie: ", document.cookie);
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>
                    Sign in with your Google Account
                </h4>
                <MyInput ref="loginGoogleId" id="GoogleId" label="Google ID"/>
                <MySubmit ref="loginButton" onClick={this.handleSubmit} id="loginButton" value="Sing In"/>

                <p className="text-center">
                    Don't have an account?
                    <a href="/#/register">Create a new one!</a>
                </p>
            </div>
        );
    }
}

export default LoginComponent;