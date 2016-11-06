import React, { Component } from "react";
import { Router, Route, IndexRoute } from "react-router";
import { hashHistory } from "react-router"

import Index from "./components/Index";
import App from "./components/App";
import Login from "./components/Login"
import Register from "./components/Register"


class Root extends Component {

    // We need to provide a list of routes
    // for our app, and in this case we are
    // doing so from a Root component
    render() {
        return (
            <Router history={this.props.history}>
                <Route path='/' component={App}>
                    <Route path="login" component={Login}/>
                    <Route path="register" component={Register}/>

                    <IndexRoute component={Index}/>
                </Route>
            </Router>
        );
    }
}

export default Root;