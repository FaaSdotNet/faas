import React, {Component} from "react";
import {UserListTable} from "../components/users/UsersListTable";
import {connect} from "react-redux";
@connect((store) => {
	return store;
})
export class Users extends Component {

	constructor(props)
	{
		super(props);
	}

	render()
	{
		return (
			<div>
				<h1>
					Statistics
				</h1>
				<div className="row">
					<StatisticsListsTable />
				</div>
			</div>
		);
	}
}

export default Users;