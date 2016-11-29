import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";
import {MainMenu} from "./menu/MainMenu";


class HeaderComponent extends Component {

    constructor() {
        super();
        this.state = { userId: localStorage.getItem('userId') };
        this.user = this.props.user;
    }

    render() {
        return (
            <Navbar>
                <Navbar.Header>
                    <Navbar.Brand>
                        <NavItem href="#">
                            FaaS Home
                        </NavItem>
                    </Navbar.Brand>
                </Navbar.Header>
                <Navbar.Collapse>
                    <MainMenu user={this.props.user} />
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default HeaderComponent;