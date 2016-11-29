import React, { Component } from "react";
import Header from '../components/Header'
import Footer from '../components/Footer'
import Container from '../components/Container'
import {connect} from "react-redux";

@connect((store) => {
	return store;
})
export class Dashboard extends Component {
	render() {
		return (
			<div id="main">
				<Header user={this.props.store.user}/>
				<Container page={this.props.store.page}/>
				<Footer/>
			</div>
		);
	}
}

export default Dashboard;


