import React, { Component } from "react";
import {connect} from "react-redux";

@connect((store) => {
	return store;
})
export class Footer extends Component {
	render() {

		return (
			<div id="footer">

			</div>
		);
	}
}

export default Footer;


