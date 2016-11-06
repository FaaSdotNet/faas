import React, { Component } from "react";
import { Nav, Navbar, NavItem, Header, Brand } from "react-bootstrap";
import { hashHistory } from 'react-router';
// import AuthActions from '../actions/AuthActions';
// import AuthStore from '../stores/AuthStore';


class NotLoggedIn extends Component {
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

class LoggedIn extends Component {

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
        return (
              <Nav>
                <NavItem href={`/#/users/${user.id}`}>
                    You are logged in as {user.userName}
                </NavItem>
                <NavItem href="/#/projects">
                    Projects
                </NavItem>
                <NavItem href="/#/users">
                    Users
                </NavItem>
                <NavItem onClick={this.logout}>
                    Logout
                </NavItem>
            </Nav>
            );
    }
}


class HeaderComponent extends Component {

    constructor() {
        super();
        this.state = { userId: localStorage.getItem('userId') };
    }

    componentWillMount() {
        this.setState({
            userId: localStorage.getItem('userId')
        });
        console.log(localStorage.getItem('userId'));
    }

    render() {
        return (
            <Navbar>
                <Navbar.Header>
                    <Navbar.Brand>
                        <a href="#">React Contacts</a>
                    </Navbar.Brand>
                </Navbar.Header>
                        { localStorage.getItem('userId') == null && <NotLoggedIn/> }
                        { localStorage.getItem('userId') != null && <LoggedIn/>}
            </Navbar>
        );
    }
}

export default HeaderComponent;