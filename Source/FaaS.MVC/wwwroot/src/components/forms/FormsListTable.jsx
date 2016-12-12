import React, { Component } from "react";
import {connect} from "react-redux";
import {FormsActions} from "../../actions/formsActions";
import {SessionsActions} from "../../actions/sessionsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import FormEdit from "./FormEdit";
import FormDetail from "./FormDetail";
import ElementCreate from "../elements/ElementCreate";
import {ElementsActions} from "../../actions/elementsActions";
import {PagesActions} from "../../actions/pagesActions";

@connect((store) => {
	return store;
})
export class FormListRow extends Component{

	/**
	 * Creates List table row
	 * @param props
	 * @property {Object} project
	 */
	constructor(props) {
		super(props);
		this.state = {
			editOpen:  {open: false},
			addElement: {open: false},
			viewForm: {open: false},
			detailOpen: {open: false}
		};

		this.handleAddForm = this.handleAddForm.bind(this);
		this.handleFormClick = this.handleFormClick.bind(this);
		this.handleEditClick = this.handleEditClick.bind(this);
		this.handleDeleteForm = this.handleDeleteForm.bind(this);
		this.handleViewForm = this.handleViewForm.bind(this);
	}


	/**
	 * Will open form list for projectId
	 * @param formId Project ID
	 */
	handleFormClick(formId)
	{
        /*
         TODO
         Show modal with new Form
         */

		this.props.dispatch(ElementsActions.reset());
		this.props.dispatch(PagesActions.setForm(formId));
		document.location.href="/#/elements/";
	}

	/**
	 * Will open form list for projectId
	 * @param projectId Project ID
	 */
	handleEditClick()
	{
		this.setState({ editOpen: { open: true }});
	}

	/**
	 * Adds form to specified project
	 * @param projectId Project Id
	 */
	handleAddForm() {
		this.setState({ addElement: { open: true }});
	}


	handleViewForm(formId) {
		document.location.href=`/#/form/${formId}`;
	}

	handleViewSessions(formId) {
		this.props.dispatch(SessionsActions.reset());
		this.props.dispatch(PagesActions.setSessions(formId));
		document.location.href="/#/sessions/";
	}

	handleDeleteForm(formId)
	{
		this.props.dispatch(FormsActions.del(formId, this.props.user.userId));
	}

	closeModal()
	{
		this.setState({
			addElement: false,
			editOpen: false
		});
	}

	/**
	 *
	 * @returns {XML}
	 */
	render(){
		return (
            <tr>
                <td>
                    <a class="details" onClick={() => this.handleFormClick(this.props.form.id)}>
						{this.props.form.formName}
                    </a>
                </td>
                <td>
					{this.props.form.numElems}
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleViewSessions(this.props.form.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-list-alt" aria-hidden="true"/>
					</button>
				</td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleViewForm(this.props.form.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-search" aria-hidden="true"/>
					</button>
				</td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleAddForm(this.props.forms.id)}>
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-plus" aria-hidden="true"/>
                    </button>
                    <ModalWrapper title="Create Element" open={this.state.addElement}  >
                        <ElementCreate formId={this.props.form.id} closeModal={this.closeModal.bind(this)} />
                    </ModalWrapper>
                </td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
                    </button>
                    <ModalWrapper title="Edit Project" open={this.state.editOpen}  >
                        <FormEdit form={this.props.form} closeModal={this.closeModal.bind(this)} />
                    </ModalWrapper>
                </td>
                <td>
                    <ButtonDelete item={this.props.form} handleDelete={this.handleDeleteForm} />
                </td>
            </tr>
		);
	}
}


@connect((store) => {
	return store;
})
export class FormsListTable extends Component {
	
	/**
	 * @param props
	 * @property {Array} forms
	 */
	constructor(props){
		super(props);
		this.rows = [];
	}

	render(){
		if(this.props.forms.reload) {
			this.props.dispatch(FormsActions.fetchAll(this.props.page.projectId, this.props.user.userId));
		}

		this.rows = [];
		const forms = this.props.forms.forms;
		if (forms) {
			forms.forEach((form) =>
			{
				this.rows.push(<FormListRow key={form.id} form={form}/>)
			});
		}

		return (
            <div className="row" id="forms">
                <h1>
                    Forms
                </h1>
                <table className="table table-striped row">
                    <thead>
                    <tr>
                        <th>Form name</th>
                        <th>Number of elements</th>
						<th>View Sessions</th>
                        <th>View form</th>
                        <th>Add element</th>
                        <th>Edit</th>
                        <th>Delete</th>
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

export default FormsListTable;