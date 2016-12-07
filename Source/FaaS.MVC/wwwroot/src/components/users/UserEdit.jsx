import React, { Component } from "react";
import {connect} from "react-redux";
import {UsersActions} from "../../actions/usersActions";

@connect((store) => {
    return store;
})
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
        this.setState({
            userName: event.target.value
        });
    }

    handleGoogleIdChange(event) {
        this.setState({
            googleId: event.target.value
        });
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/users/' + this.props.userPassed.id,
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

        const payload = {
			Id: this.props.userPassed.id,
			GoogleId: googleId,
			UserName: userName,
			Registered: this.state.registered
		};

        this.props.dispatch(UsersActions.update(payload));
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

                <br/>
                <input type="button" 
                        id="editButton"
                        onClick={this.handleSubmit}
                        value="Save" 
                        className="btn btn-primary col-md-offset-5"/>

            </div>
        );
    }
}

export default UserEdit;