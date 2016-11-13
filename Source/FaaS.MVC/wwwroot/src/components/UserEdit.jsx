import React, { Component } from "react";

class UserEdit extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleUserNameChange = this.handleUserNameChange.bind(this);
        this.handleGoogleIdChange = this.handleGoogleIdChange.bind(this);

        this.state = {};
        this.state.googleId = "";
        this.state.userName = "";
        this.state.registered = "";
    }

    handleUserNameChange(event) {
        this.setState({userName: event.target.value,
            googleId: this.state.googleId,
            registered: this.state.registered});
    }

    handleGoogleIdChange(event) {
        this.setState({userName: this.state.userName,
            googleId: event.target.value,
            registered: this.state.registered});
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
            if(res.ok) {
                res.json()
                    .then((js) => {
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        const googleId = this.state.googleId;
        const userName = this.state.userName;
        var result = fetch('/api/v1.0/users', {
            method: 'PUT',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: this.props.params.userid,
                GoogleId: googleId,
                UserName: userName,
                Registered: this.state.registered
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
                <h4>Edit User</h4>
                <label htmlFor="googleId" className="col-md-5 control-label">
                    Google ID
                </label>
                <input ref="editGoogleId" type="text" id="GoogleId"
                       onChange={this.handleGoogleIdChange} className="form-control"
                       value={this.state.googleId} />
                <label htmlFor="userName" className="col-md-5 control-label">
                    User Name
                </label>
                <input ref="editUserName" type="text" id="UserName"
                       onChange={this.handleUserNameChange} className="form-control"
                       value={this.state.userName} />

                <input type="button" 
                        id="editButton"
                        onClick={this.handleSubmit}
                        value="Save" 
                        className="btn btn-default"/>
            </div>
        );
    }
}

export default UserEdit;