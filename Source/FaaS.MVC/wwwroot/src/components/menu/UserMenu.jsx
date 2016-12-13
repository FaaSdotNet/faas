import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";
import {connect} from "react-redux";
import {UserActions} from "../../actions/userActions";

@connect((store) => {
	return store;
})
export class UserMenu extends Component {
	render() {

		let projects = null;
		let forms = null;
		let elements = null;
		let sessions = null;
		let about = null;
		let faq = null;
		
		if (!this.props.user.userId)
		{
			faq = (
				<NavItem href="/#/faq">
					FAQ
				</NavItem>
			);
			about = (
				<NavItem href="/#/about">
					About
				</NavItem>
			);
		}
		else
		{
			projects = (
				<NavItem href="/#/projects">
					Projects
				</NavItem>
			);
		}

		if (this.props.page.projectId) {
			forms = (
				<NavItem href="/#/forms">
					Forms
				</NavItem>
			);
		}

		if (this.props.page.formId) {
			elements = (
				<NavItem href="/#/elements">
					Elements
				</NavItem>
			);
			sessions = (
				<NavItem href="/#/sessions">
					Sessions
				</NavItem>
			);
		}


		return(
			<Nav>
				{projects}
				{forms}
				{elements}
				{sessions}
				{faq}
				{about}
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
		document.location.href = "/welcome";
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
