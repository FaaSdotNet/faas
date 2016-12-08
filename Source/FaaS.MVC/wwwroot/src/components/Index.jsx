import React, { Component } from 'react';
import {connect} from "react-redux";
import {UserActions} from "../actions/userActions";

@connect( (store) => {
    return store;
})
export class IndexComponent extends Component {

    constructor(props) {
        super(props);

        // Obtain token from local storage, where it was set by Welcome page
        const userToken = localStorage.getItem("GoogleToken");

        if (userToken) {
            const payload = {
                GoogleToken: userToken
            }
            this.props.dispatch(UserActions.logIn(payload));
            document.location.href = "/#/projects";
        }

        /*
        if (userToken) {
            const result = fetch('/api/v1.0/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: "same-origin",

                body: JSON.stringify({
                    GoogleToken: userToken
                })
            });

            result.then((res) =>{    
                if (res.ok) {
                    res.json()
                        .then((js) => {
                            this.props.dispatch(UserActions.loginSuccess(userToken));
                            document.location.href ="/#/projects";
                        });
                }
            });
        }
        */
    }
    render() {
                return (
                    <div className="row">
                <h3 className="text-center">
                    Welcome to Form as a Service!
                </h3>
                <h4 className="text-center">
                    It appears you are not signed in, please visit <a href="/welcome">Welcome page</a> to sign in.
                </h4>
            </div>
            ); 
        } 
}

export default IndexComponent;