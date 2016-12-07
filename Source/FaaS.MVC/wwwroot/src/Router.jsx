import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login";
import Register from "./components/Register";
import Form from "./components/Form";
import Projects from "./pages/Projects";
import Sessions from "./pages/Sessions";
import Forms from "./pages/Forms";
import Elements from "./pages/Elements";
import Users from "./pages/Users";
import About from "./pages/About";
import FAQ from "./pages/FAQ";


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
                    <Route path="users" component={Users} />
                    <Route path="sessions" component={Sessions} />
                    <Route path="about" component={About} />
                    <Route path="faq" component={FAQ} />
                    <Route path="form/:formid" component={Form} />
                    <IndexRoute component={Index} />
                </Route>
            </Router>
        );
                }
}

export default Root;