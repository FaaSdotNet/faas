import React, {Component} from "react";
import {connect} from "react-redux";
import {UsersActions} from "../../actions/usersActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import UserEdit from "./UserEdit";
import UserDetail from "./UserDetail";


@connect((store) =>
{
	return store;
})
export class UserListRow extends Component {

	/**
	 * Creates List table row
	 * @param props
	 * @property {Object} user
	 */
	constructor(props)
	{
		super(props);
		this.state = {
			editOpen: {open: false},
			detailOpen: {open: false}
		};

		this.handleUserClick = this.handleUserClick.bind(this);
		this.handleEditClick = this.handleEditClick.bind(this);
		this.handleDeleteUser = this.handleDeleteUser.bind(this);
	}


	/**
	 * Will open form list for userId
	 * @param userId User ID
	 */
	handleUserClick()
	{
		this.setState({detailOpen: {open: true}});
	}

	/**
	 * Will open form list for userId
	 * @param userId User ID
	 */
	handleEditClick()
	{
		this.setState({editOpen: {open: true}});
	}


	handleDeleteUser(userId)
	{
		this.props.dispatch(UsersActions.del(userId));
	}

	render()
	{
		return (
			<tr>
				<td>
					<a onClick={() => this.handleUserClick()}>
						{this.props.userPassed.userName}
					</a>
					<ModalWrapper title="User Detail" open={this.state.detailOpen}>
						<UserDetail userPassed={this.props.userPassed}/>
					</ModalWrapper>
				</td>
				<td>
					{this.props.userPassed.googleId}
				</td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Edit User" open={this.state.editOpen}>
						<UserEdit userPassed={this.props.userPassed}/>
					</ModalWrapper>
				</td>
				<td>
					<ButtonDelete item={this.props.userPassed} handleDelete={this.handleDeleteUser}/>
				</td>
			</tr>
		);
	}
}


@connect((store) =>
{
	return store;
})
export class UserListTable extends Component {
	/**
	 * @param props
	 * @property {Array} users
	 */
	constructor(props)
	{
		super(props);
		console.log("User List Table: ", this.props);
		this.rows = [];
	}

	render()
	{
		let userId = localStorage.getItem('userId');
		if (this.props.users.reload) {
			this.props.dispatch(UsersActions.fetchAll(userId));
		}
		this.rows = [];
		const users = this.props.users.users;
		console.log("Users: ", users);
		users.forEach((user) =>
		{
			this.rows.push(<UserListRow key={user.id} userPassed={user}/>)
		});

		return (
			<div className="row" id="users-list">
				<table className="table table-striped row">
					<thead>
					<tr>
						<th>User name</th>
						<th>GoogleId</th>
						<th>Edit</th>
						<th>Delete</th>
					</tr>

					</thead>
					<tbody>
					{this.rows}
					</tbody>
				</table>
			</div>
		);
	}
}

export default UserListTable;