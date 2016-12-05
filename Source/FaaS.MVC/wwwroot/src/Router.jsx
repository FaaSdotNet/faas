import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import {connect} from "react-redux";
import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login";
import Register from "./components/Register";

import UserList from "./components/users/UserList";
import UserDetail from "./components/users/UserDetail";
import UserEdit from "./components/users/UserEdit";
import UserDelete from "./components/users/UserDelete";
import Form from "./components/Form";
import Projects from "./pages/Projects";
import Forms from "./pages/Forms";
import Elements from "./pages/Elements";


export class Root extends Component {

    constructor(props){
        super(props);
        console.log(this.props);
    }
    // We need to provide a list of routes
    // for our app, and in this case we are
    // doing so from a Root component
    render() {
		return (
            <Router history={this.props.history}>
                <Route path='/' component={App}>
                    <Route path="projects" component={Projects} />
                    <Route path="forms" component={Forms} />
					<Route path="elements" component={Elements} />
					<Route path="login" component={Login} />
                    <Route path="register" component={Register} />
                    <Route path="users/:userid" component={UserDetail} />
                    <Route path="users/edit/:userid" component={UserEdit} />
                    <Route path="users/delete/:userid" component={UserDelete} />
                    <Route path="users" component={UserList} />
                    <Route path="form/:formid" component={Form} />
                    <IndexRoute component={Index} />
                </Route>
            </Router>
        );
                }
}

export default Root;