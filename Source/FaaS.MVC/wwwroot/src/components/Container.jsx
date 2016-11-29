import React, { Component } from "react";
import {connect} from "react-redux";

@connect((store) => {
	return store;
})
export class Container extends Component {
	constructor(){
		/** @type Object */
		this.page = this.props.page;
	}

	render() {
		return (
			<div id="container">

			</div>
		);
	}
}

export default Container;


