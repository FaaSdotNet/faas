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
            };
            this.props.dispatch(UserActions.logIn(payload));
        }
    }
    render() {

            if(this.props.user.userId){
				document.location.href = "/#/projects";
			}
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