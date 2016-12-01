import React, { Component } from "react";
import Header from '../components/Header'
import Footer from '../components/projects/ProjectListTable'
import Container from './Container'
import {connect} from "react-redux";

@connect((store) => {
    return store;
})
export class Projects extends Component {

    constructor(props){
        super(props);
        this.rows = null;
    }

    render() {
        return (
            <div id="projects">

            </div>
        );
    }
}

export default Projects;
