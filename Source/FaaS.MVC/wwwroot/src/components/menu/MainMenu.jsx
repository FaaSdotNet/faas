/**
 * Created by Wermington on 27.11.16.
 */

import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";
import {LoginMenu} from "./LoginMenu";
import {LoggedInUser, UserMenu} from "./UserMenu";


export class MainMenu extends Component {
	render() {
		const isLoggedIn = this.props.user != null;
		return(
			<Nav>
				<NavItem href="/#/About">
					About
				</NavItem>
				<NavItem href="/#/Contact">
					Contact
				</NavItem>
				<NavItem href="/#/FAQ">
					FAQ
				</NavItem>
				{ isLoggedIn ? (
					<UserMenu/>
				) : (
					<LoginMenu/>
				)}

				{ isLoggedIn && <LoggedInUser/> }

			</Nav>
		);
	}
}




