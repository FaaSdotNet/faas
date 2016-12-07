import React, {Component} from "react";
import {SessionListTable} from "../components/sessions/SessionsListTable";
import {connect} from "react-redux";
@connect((store) => {
	return store;
})
export class Sessions extends Component {

	constructor(props)
	{
		super(props);
	}

	render()
	{
		return (
			<div>
				<h1>
					Sessions
				</h1>
				<div className="row">
					<SessionsListTable />
				</div>
			</div>
		);
	}
}

export default Sessions;