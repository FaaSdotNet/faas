import React, { Component } from 'react';
import ProjectList from './ProjectList'

class Dashboard extends Component {

    constructor(props) {
        super(props);
        const user = localStorage.getItem("user");
        console.log("User: ", user);

        if(user == null) {
             document.location.href = "/#/index";
             console.log("Redirecting from dashboard");
         }
    }



    render() {
        return (
        <div className="col-md-12 col-xs-12 row">
                <h1>
                    Welcome to Form as a Service!
                </h1>
            <ProjectList />
          </div>
      );
    }
}

export default Dashboard;