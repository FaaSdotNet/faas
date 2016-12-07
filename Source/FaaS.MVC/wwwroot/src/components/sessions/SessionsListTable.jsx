import React, { Component } from "react";
import {connect} from "react-redux";
import {SessionsActions} from "../../actions/sessionsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
//import {ElementsActions} from "../../actions/elementsActions";
import {PagesActions} from "../../actions/pagesActions";

@connect((store) => {
	return store;
})
export class SessionsListTable extends Component{
	/**
	 * @param props
	 * @property {Array} sessions
	 */
	constructor(props){
		super(props);
		this.sessions = [];
	}

	render(){
		let userId = localStorage.getItem('userId');

		if (this.props.sessions.reload) {
			this.props.dispatch(SessionsActions.fetchAll(this.props.page.formId));
		}

		this.rows = [];
		const sessions = this.props.sessions.sessions;
		if (sessions) {
			sessions.forEach((sessions) =>
			{
				//this.rows.push(<SessionListRow key={session.id} session={session}/>)
			});
		}

		return (
            <div className="row" id="sessions">
                <h1>
                    Sessions
                </h1>
            </div>
		);
	}
}

export default SessionsListTable;