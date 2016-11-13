import React, { Component } from 'react';
import Login from './Login';

class IndexComponent extends Component {

    constructor() {
        super();
        const user = localStorage.getItem("user");
        if (user) {
            document.location.href = "/#/dashboard";
            console.log("Redirecting from index");
        }
    }
    render() {
        return (
            <div className="row">
                <h1>
                    Welcome to Form as a Service!
                </h1>
                <Login />
            </div>    
      );
    }
}

export default IndexComponent;