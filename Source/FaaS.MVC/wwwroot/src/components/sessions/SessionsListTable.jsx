import React, { Component } from "react";
import {connect} from "react-redux";
import {SessionsActions} from "../../actions/sessionsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import {PagesActions} from "../../actions/pagesActions";

@connect((store) => {
	return store;
})
export class SessionListRow extends Component{

	/**
	 * Creates List table row
	 * @param props
	 * @property {Object} session
	 */
	constructor(props) {
		super(props);

		this.handleViewSession = this.handleViewSession.bind(this);
	}

	handleViewSession(sessionId) {
		document.location.href=`/#/form/${sessionId}`;
	}

	/**
	 *
	 * @returns {XML}
	 */
	render(){
		return (
            <tr>
                <td>
                   {this.props.session.filled}
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleViewSession(this.props.session.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-search" aria-hidden="true"/>
					</button>
				</td>
            </tr>
		);
	}
}


@connect((store) => {
	return store;
})
export class SessionsListTable extends Component {
	/**
	 * @param props
	 * @property {Array} sessions
	 */
	constructor(props) {
		super(props);
		this.rows = [];
	}

	render() {

		if (this.props.sessions.reload) {
			this.props.dispatch(SessionsActions.fetchAll(this.props.page.formId));
		}

		this.rows = [];
		const sessions = this.props.sessions.sessions;
		if (sessions) {
			sessions.forEach((session) =>
			{
				this.rows.push(<SessionListRow key={session.id} session={session}/>)
			});
		}

		return (
            <div className="row" id="sessions">
				<table className="table table-striped row">
                    <thead>
                    <tr>
                        <th>Submitted</th>
                        <th>View Session</th>
                    </tr>

                    </thead>
                    <tbody>
					{this.rows}
                    </tbody>
                </table>
            </div>
		);
	}
}

export default SessionsListTable;