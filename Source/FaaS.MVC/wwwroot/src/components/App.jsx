// src/components/App.js

import "normalize.css/normalize.css";
import "bootstrap/dist/css/bootstrap.min.css";

import React, { Component } from "react";
import Header from "./Header";
import { Grid, Row, Col } from "react-bootstrap";

class AppComponent extends Component {

    render() {
        return (
            <div>
                <Header lock={this.lock}></Header>
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