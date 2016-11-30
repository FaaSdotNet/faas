import React, { Component } from "react";
import MyInput from "./form/MyInput"
import cookie from 'react-cookie'


export class LoginComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
        if (localStorage.getItem("user") != null) {
            document.location.href = "/#/dashboard";
        }
    }

    handleSubmit(event) {
        const googleId = this.refs.loginGoogleId.state.value;
        
        const result = fetch('/api/v1.0/login', {
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
                if (res.ok) {
                    res.json()
                        .then((js) => {
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
            <div className="form-horizontal text-center">
                <h4>
                    Sign in with your Google Account
                </h4>
                <br/>
                <MyInput ref="loginGoogleId" id="GoogleId" label="Google ID"/>
                <div className="form-group">
                    <div className="col-md-offset-1 col-md-10">
                        <input type="button" 
                            id="loginButton"
                            onClick={this.handleSubmit}
                            value="Sign In" 
                            className="btn btn-default"/>
                    </div>
                </div>

                <p className="text-center">
                    Don't have an account?
                    <a href="/#/register"> Create a new one!</a>
                </p>
            </div>
        );
    }
}

export default LoginComponent;