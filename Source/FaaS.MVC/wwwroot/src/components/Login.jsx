import React, { Component } from "react";
import MyInput from "./form/MyInput"
import MySubmit from "./form/MySubmit"
import cookie from 'react-cookie'
import { hashHistory } from 'react-router';
import {User} from "../entities/User";




export class LoginComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        if (localStorage.getItem("user") != null) {
            document.location.href = "/#/dashboard";
            console.log("Redirecting from login.");
        }
    }



    handleSubmit(event) {
        const googleId = this.refs.loginGoogleId.state.value;
        console.log('Logging in as: ', googleId);
        var result = fetch('/api/v1.0/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: "same-origin",

                body: JSON.stringify({
                    GoogleId: googleId
                })
            });
            result.then( (res) =>  {
                console.log(res);
                
                if (res.ok) {

                    res.json()
                        .then((js) => {
                            console.log(js);
                            console.info("UserID: ", js.id);
                            cookie.save("userId", js.id);
                            localStorage.setItem("userId", js.id);
                            localStorage.setItem("user", JSON.stringify(js));
                            document.location.href ="/#/dashboard";
                        });
                }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>
                    Sign in with your Google Account
                </h4>
                <MyInput ref="loginGoogleId" id="GoogleId" label="Google ID"/>
                <MySubmit ref="loginButton" onClick={this.handleSubmit} id="loginButton" value="Sign In"/>

                <p className="text-center">
                    Don't have an account?
                    <a href="/#/register">Create a new one!</a>
                </p>
            </div>
        );
    }
}

export default LoginComponent;