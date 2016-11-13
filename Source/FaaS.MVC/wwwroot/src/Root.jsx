import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import { hashHistory } from "react-router"

import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login";
import Register from "./components/Register";
import ProjectList from "./components/ProjectList";
import ProjectCreate from "./components/ProjectCreate";
import Dashboard from "./components/Dashboard";
import UserList from "./components/UserList";
import UserDetail from "./components/UserDetail";
import UserEdit from "./components/UserEdit";
import UserDelete from "./components/UserDelete";
import ProjectDetail from "./components/ProjectDetail";



class Root extends Component {

    // We need to provide a list of routes
    // for our app, and in this case we are
    // doing so from a Root component
    render() {
        return (
            <Router history={this.props.history}>
                <Route path='/' component={App}>
                    <Route path="index" component={Index} />
                    <Route path="dashboard" component={Dashboard} />
                    <Route path="login" component={Login} />
                    <Route path="register" component={Register} />
                    <Route path="projects" component={ProjectList} />
                    <Route path="projects/create" component={ProjectCreate} />
                    <Route path="projects/:projectid" component={ProjectDetail} />
                    <Route path="users/:userid" component={UserDetail} />
                    <Route path="users/edit/:userid" component={UserEdit} />
                    <Route path="users/delete/:userid" component={UserDelete} />
                    <Route path="users" component={UserList} />

                    <IndexRoute component={Index} />
                </Route>
            </Router>
        );
                }
}

export default Root;