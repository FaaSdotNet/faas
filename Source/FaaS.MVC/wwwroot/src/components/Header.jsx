import React, { Component } from "react";
import { Nav, Navbar, NavItem, NavDropdown, MenuItem, Header, Brand } from "react-bootstrap";
import { hashHistory } from 'react-router';
// import AuthActions from '../actions/AuthActions';
// import AuthStore from '../stores/AuthStore';


class NotLoggedInMenu extends Component {
    render() {
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
            </Nav>
        );
    }
}

class LoggedInMenu extends Component {
    render() {
        return(
            <Nav>
                <NavItem href="/#/projects">
                    Projects
                </NavItem>
                <NavItem href="/#/forms">
                    Forms
                </NavItem>
            </Nav>
        );
    }
}

class LoggedInUser extends Component {

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


class HeaderComponent extends Component {

    constructor() {
        super();
        this.state = { userId: localStorage.getItem('userId') };
    }

    render() {
        var menu = <Navbar.Collapse><NotLoggedInMenu/></Navbar.Collapse>;
        if (localStorage.getItem("userId") != null)
        {
            menu = <Navbar.Collapse><LoggedInMenu/><LoggedInUser/></Navbar.Collapse>;
        }
        return (
            <Navbar>
                <Navbar.Header>
                    <Navbar.Brand>
                        <a href="#">FaaS Home</a>
                    </Navbar.Brand>
                </Navbar.Header>
                <Navbar.Collapse>
                    {menu}
                </Navbar.Collapse>
            </Navbar>
        );
    }
}

export default HeaderComponent;