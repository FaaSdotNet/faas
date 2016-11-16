import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import { hashHistory } from "react-router"

import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login";
import Register from "./components/Register";
import Dashboard from "./components/Dashboard";
import UserList from "./components/users/UserList";
import UserDetail from "./components/users/UserDetail";
import UserEdit from "./components/users/UserEdit";
import UserDelete from "./components/users/UserDelete";
import ProjectList from "./components/projects/ProjectList";
import ProjectCreate from "./components/projects/ProjectCreate";
import ProjectDetail from "./components/projects/ProjectDetail";
import ProjectEdit from "./components/projects/ProjectEdit";
import ProjectDelete from "./components/projects/ProjectDelete";
import FormList from "./components/forms/FormList";
import FormCreate from "./components/forms/FormCreate";
import FormEdit from "./components/forms/FormEdit";
import FormDetail from "./components/forms/FormDetail";
import FormDelete from "./components/forms/FormDelete";
import ElementList from "./components/elements/ElementList";
import ElementCreate from "./components/elements/ElementCreate";
import ElementEdit from "./components/elements/ElementEdit";
import ElementDetail from "./components/elements/ElementDetail";
import ElementDelete from "./components/elements/ElementDelete";

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
                    <Route path="users/:userid" component={UserDetail} />
                    <Route path="users/edit/:userid" component={UserEdit} />
                    <Route path="users/delete/:userid" component={UserDelete} />
                    <Route path="users" component={UserList} />
                    <Route path="projects" component={ProjectList} />
                    <Route path="projects/create" component={ProjectCreate} />
                    <Route path="projects/:projectid" component={ProjectDetail} />
                    <Route path="projects/edit/:projectid" component={ProjectEdit} />
                    <Route path="projects/delete/:projectid" component={ProjectDelete} />
                    <Route path="forms" component={FormList} />
                    <Route path="forms/create" component={FormCreate} />
                    <Route path="forms/:formid" component={FormDetail} />
                    <Route path="forms/edit/:formid" component={FormEdit} />
                    <Route path="forms/delete/:formid" component={FormDelete} />
                    <Route path="elements" component={ElementList} />
                    <Route path="elements/create" component={ElementCreate} />
                    <Route path="elements/:elementid" component={ElementDetail} />
                    <Route path="elements/edit/:elementid" component={ElementEdit} />
                    <Route path="elements/delete/:elementid" component={ElementDelete} />

                    <IndexRoute component={Index} />
                </Route>
            </Router>
        );
                }
}

export default Root;