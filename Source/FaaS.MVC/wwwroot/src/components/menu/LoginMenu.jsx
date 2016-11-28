import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";

export class LoginMenu extends Component {
	render() {
		return(
			<Nav>
				<NavItem href="/#/login">
					Login
				</NavItem>
				<NavItem href="/#/register">
					Register
				</NavItem>
			</Nav>
		);
	}
}
