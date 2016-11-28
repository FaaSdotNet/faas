import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";



export class UserMenu extends Component {
	render() {
		return(
			<Nav>
				<NavItem href="/#/dashboard">
					Dashboard
				</NavItem>
				<NavItem href="/#/projects">
					Projects
				</NavItem>
			</Nav>
		);
	}
}



export class LoggedInUser extends Component {

	constructor() {
		super();
		this.logout = this.logout.bind(this);
	}


	logout() {

		localStorage.removeItem('user');
		localStorage.removeItem('userId');
		/** Clear all cookies starting with 'session' (to get all cookies, omit regex argument) */
		document.location.reload();
	}

	render() {
		const user = JSON.parse(localStorage.getItem("user"));

		let title = <div><img style={{display: "inline"}} className="img-responsive img-circle" src="../images/default_user.png" alt="defaut_user" height="24" width="24" />
			&nbsp;{user.userName}</div>;

		return (
			<Nav pullRight>
				<NavDropdown noCaret id="userDropdown" title={title}>
					<MenuItem href={`/#/users/${user.id}`} >
						Details
					</MenuItem>
					<MenuItem divider />
					<MenuItem onClick={this.logout}>
						Log Out
					</MenuItem>
				</NavDropdown>
			</Nav>
		);
	}
}
