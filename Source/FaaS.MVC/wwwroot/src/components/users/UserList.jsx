import React, { Component } from "react";
import MyViewTable from "../table/MyViewTable"


export class UserList extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="row">
                <div className="row">
                </div>
            <MyViewTable name="users" propName="userName" />
            </div>
        );
    }
}

export default UserList;
