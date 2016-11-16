import React, { Component } from "react";

class UserDelete extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);

        this.state = {};
        this.state.googleId = "";
        this.state.userName = "";
        this.state.registered = "";
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/users/' + this.props.params.userid,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json'
            }
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        var result = fetch('/api/v1.0/users/' + this.props.params.userid,
        {
            method: 'DELETE',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
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
                <h4>Delete User</h4>
                <p>Delete user with User Name: {this.state.userName} ?</p>

                <input type="button" 
                    id="deleteButton"
                    onClick={this.handleSubmit}
                    value="Delete" 
                    className="btn btn-default"/>
            </div>
        );
    }
}

export default UserDelete;