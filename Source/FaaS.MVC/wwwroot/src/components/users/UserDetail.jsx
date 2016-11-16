import React, { Component } from "react";


export class UserDetail extends Component {

    constructor(props) {
        super(props);

        this.state = null;
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

    render() {
        var rows =[];
        var state = this.state;
        if (state) {
            rows.push(<p key={state.userName}><b>User Name: </b>{state.userName}</p>);
            rows.push(<p key={state.googleId}><b>Google ID: </b>{state.googleId}</p>);
            rows.push(<p key={state.registered}><b>Registered: </b>{state.registered}</p>);
        }

        return (
            <div>
                <h2>User Details</h2>
                {rows}
            </div>
        );
    }
}

export default UserDetail;