import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import {connect} from "react-redux";
import Dashboard from "./components/Dashboard";

import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login";
import Register from "./components/Register";

import UserList from "./components/users/UserList";
import UserDetail from "./components/users/UserDetail";
import UserEdit from "./components/users/UserEdit";
import UserDelete from "./components/users/UserDelete";
import ProjectList from "./components/projects/ProjectList";
import ProjectDetail from "./components/projects/ProjectDetail";
import FormCreate from "./components/forms/FormCreate";
import FormEdit from "./components/forms/FormEdit";
import FormDetail from "./components/forms/FormDetail";
import FormDelete from "./components/forms/FormDelete";
import ElementCreate from "./components/elements/ElementCreate";
import ElementEdit from "./components/elements/ElementEdit";
import ElementDetail from "./components/elements/ElementDetail";
import ElementDelete from "./components/elements/ElementDelete";
import Form from "./components/Form"

@connect((store) => {
    console.log(store);
    return store;
})
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
                <Route path='/' component={App} user={this.props.user}>
                    <Route path="index" component={Index} />
                    <Route path="dashboard" component={Dashboard} user="" />
                    <Route path="login" component={Login} />
                    <Route path="register" component={Register} />
                    <Route path="users/:userid" component={UserDetail} />
                    <Route path="users/edit/:userid" component={UserEdit} />
                    <Route path="users/delete/:userid" component={UserDelete} />
                    <Route path="users" component={UserList} />
                    <Route path="projects" component={ProjectList} />
                    <Route path="projects/:projectid" component={ProjectDetail} />
                    <Route path="forms/create/:projectId" component={FormCreate} />
                    <Route path="forms/:formid" component={FormDetail} />
                    <Route path="forms/edit/:formid" component={FormEdit} />
                    <Route path="forms/delete/:formid" component={FormDelete} />
                    <Route path="elements/create/:formid" component={ElementCreate} />
                    <Route path="elements/:elementid" component={ElementDetail} />
                    <Route path="elements/edit/:elementid" component={ElementEdit} />
                    <Route path="elements/delete/:elementid" component={ElementDelete} />
                    <Route path="form/:formid" component={Form} />
                    <IndexRoute component={Index} />
                </Route>
            </Router>
        );
                }
}

export default Root;