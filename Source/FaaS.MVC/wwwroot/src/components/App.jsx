// src/components/App.js

import "normalize.css/normalize.css";
import "bootstrap/dist/css/bootstrap.min.css";

import React, { Component } from "react";
import Header from "./Header";
import { Grid, Row, Col } from "react-bootstrap";
import {connect} from "react-redux";

@connect((store) =>  {
	return store;
})
class AppComponent extends Component {

    constructor(props){
        super(props);
    }

    render() {
        return (
            <div>
                <Header user={this.props.user} lock={this.lock}/>
                <Grid>
                    <Row>
                       {this.props.children}
                    </Row>
                </Grid>
            </div>
        );
    }
}

export default AppComponent;