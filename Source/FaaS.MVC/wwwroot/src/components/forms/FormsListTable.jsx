import React, { Component } from "react";
import {connect} from "react-redux";
import {FormsActions} from "../../actions/formsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import FormEdit from "./FormEdit";
import FormDetail from "./FormDetail";
import ElementCreate from "../elements/ElementCreate";

@connect((store) => {
	return store;
})
export class FormListRow extends Component{

	/**
	 * Creates List table row
	 * @param props
	 * @property {Object} project
	 */
	constructor(props){
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
	}


	/**
	 * Will open form list for projectId
	 * @param projectId Project ID
	 */
	handleFormClick(formId)
	{
        /*
         TODO
         Show modal with new Form
         */

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
	handleAddForm(){
		this.setState({ addElement: { open: true }});


	}

	handleDeleteForm(formId)
	{
		console.log("[DELETE] Project: ", formId);
		this.props.dispatch(FormsActions.del(formId));
	}

	/**
	 *
	 * @returns {XML}
	 */

	render(){
		return (
            <tr>
                <td>
                    <a onClick={() => this.handleFormClick(this.props.forms.id)}>
						{this.props.form.formName}
                    </a>
                </td>
                <td>
					{this.props.form.numElems}
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleAddForm(this.props.forms.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-search" aria-hidden="true"/>
					</button>
					<ModalWrapper title="View Form" open={this.state.viewForm}  >
						<div form={this.props.form} />
					</ModalWrapper>
				</td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleAddForm(this.props.forms.id)}>
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-plus" aria-hidden="true"/>
                    </button>
                    <ModalWrapper title="Create element" open={this.state.addElement}  >
                        <ElementCreate form={this.props.form} />
                    </ModalWrapper>
                </td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
                    </button>
                    <ModalWrapper title="Edit Project" open={this.state.editOpen}  >
                        <FormEdit form={this.props.form} />
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
export class FormsListTable extends Component{
	/**
	 * @param props
	 * @property {Array} forms
	 */
	constructor(props){
		super(props);
		console.log("Forms List Table: ", this.props);
		this.rows = [];
	}



	render(){
		let userId = localStorage.getItem('userId');

		if(this.props.forms.reload) {
			this.props.dispatch(FormsActions.fetchAll(this.props.page.projectId));
		}

		this.rows = [];
		const forms = this.props.forms.forms;
		console.log("Forms: ", forms);
		if(forms) {
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