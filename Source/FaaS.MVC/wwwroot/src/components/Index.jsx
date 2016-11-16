import React, { Component } from 'react';
import Login from './Login';

class IndexComponent extends Component {

    constructor() {
        super();
        const user = localStorage.getItem("user");
        if (user) {
            document.location.href = "/#/dashboard";
        }
    }
    render() {
        return (
            <div className="row">
                <h3 className="text-center">
                    Welcome to Form as a Service! Sign In to continue.
                </h3>
                <br/>
                <img className="img-responsive center-block img-circle"
                    src="./images/default_user.png" alt="defaut_user" height="150" width="150"/>
                <Login />
            </div>    
      );
    }
}

export default IndexComponent;