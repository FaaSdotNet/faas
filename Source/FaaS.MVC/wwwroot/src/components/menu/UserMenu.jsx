import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";
import {connect} from "react-redux";
import {UserActions} from "../../actions/userActions";

export class UserMenu extends Component {
	render() {
		return(
			<Nav>
				<NavItem href="/#/projects">
					Projects
				</NavItem>
			</Nav>
		);
	}
}


@connect( (store) => {
    return store;
})
export class LoggedInUser extends Component {

	constructor(props) {
		super(props);
		this.logout = this.logout.bind(this);
	}


	logout() {
	    this.props.dispatch(UserActions.logout());
		/** Clear all cookies starting with 'session' (to get all cookies, omit regex argument) */
		document.location.reload();
	}

	render() {
		const user = this.props.user;

		let title = <div><img style={{display: "inline"}}
							  className="img-responsive img-circle"
							  src={user.avatarUrl}
							  alt="defaut_user" height="24" width="24" />
			&nbsp;{user.name}</div>;

		return (
			<Nav pullRight>
				<NavDropdown noCaret id="userDropdown" title={title}>
					<MenuItem onClick={this.logout}>
						Log Out
					</MenuItem>
				</NavDropdown>
			</Nav>
		);
	}
}
