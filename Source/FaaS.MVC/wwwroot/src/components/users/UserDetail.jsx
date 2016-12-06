import React, { Component } from "react";


export class UserDetail extends Component {

    constructor(props) {
        super(props);

        this.state = {};
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

    render() {
        const state = this.state;
        return (
            <div>
                <h2>User Details</h2>
                <div>
                    <p>
                        <b>User Name: </b> {state.userName}
                    </p>
                    <p>
                        <b>Google ID: </b> {state.googleId}
                    </p>
                    <p>
                        <b>Registered: </b>{state.registered}
                    </p>

                </div>
            </div>
        );
    }
}

export default UserDetail;