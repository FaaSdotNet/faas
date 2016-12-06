import React, { Component } from "react";
import {connect} from "react-redux";
import {ElementsActions} from "../../actions/elementsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import ElementEdit from "./ElementEdit";
import ElementDetail from "./ElementDetail";

@connect((store) => {
	return store;
})
export class ElementListRow extends Component{

	/**
	 * Creates List table row
	 * @param props
	 * @property {Object} element
	 */
	constructor(props){
		super(props);
		this.state = {
			editOpen:  {open: false},
			addElement: {open: false},
			detailOpen: {open: false}
		};

		this.handleAddElement = this.handleAddElement.bind(this);
		this.handleElementClick = this.handleElementClick.bind(this);
		this.handleEditClick = this.handleEditClick.bind(this);
		this.handleElementDelete = this.handleElementDelete.bind(this);
	}


	/**
	 * Will open form list for elementId
	 * @param elementId Project ID
	 */
	handleElementClick(elementId)
	{
		/*
		 TODO
		 Show modal with new Form
		 */

	}

	/**
	 * Will open form list for elementId
	 * @param elementId Project ID
	 */
	handleEditClick()
	{
		this.setState({ editOpen: { open: true }});
	}



	/**
	 * Adds form to specified project
	 * @param projectId Project Id
	 */
	handleAddElement(elementId){
		this.setState({ addElement: { open: true }});
	}

	handleElementDelete(elementId)
	{
		console.log("[DELETE] Element: ", elementId);
		this.props.dispatch(ElementsActions.del(elementId));
	}

	/**
	 *
	 * @returns {XML}
	 */

	render(){
		return (
			<tr>
				<td>
					<a onClick={() => this.handleElementClick(this.props.element.id)}>
						{this.props.element.description}
					</a>
				</td>
				<td>
					{this.props.element.type}
				</td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Edit Project" open={this.state.editOpen}  >
						<ElementEdit element={this.props.element} />
					</ModalWrapper>
				</td>
				<td>
					<ButtonDelete item={this.props.element} handleDelete={this.handleElementDelete} />
				</td>
			</tr>
		);
	}
}


@connect((store) => {
	return store;
})
export class ElementsListTable extends Component{
	/**
	 * @param props
	 * @property {Array} projects
	 */
	constructor(props){
		super(props);
		console.log("Project List Table: ", this.props);
		this.rows = [];
	}



	render(){
		let userId = localStorage.getItem('userId');
		if(this.props.elements.reload) {
			this.props.dispatch(ElementsActions.fetchAll(this.props.page.formId));
		}
		this.rows = [];
		const elements = this.props.elements.elements;
		console.log("Elements: ", elements);
		elements.forEach( (element) => {
			this.rows.push(<ElementListRow key={element.id} form={element} />)
		});

		return (
			<div className="row" id="forms">
				<h1>
					Elements
				</h1>
				<table className="table table-striped row">
					<thead>
					<tr>
						<th>Element description</th>
						<th>Element type</th>
						<th>View element</th>
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

export default ElementsListTable;