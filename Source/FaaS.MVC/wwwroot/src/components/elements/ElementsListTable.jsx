import React, { Component } from "react";
import {connect} from "react-redux";
import {ElementsActions} from "../../actions/elementsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import ElementEdit from "./ElementEdit";
import {ElementType} from "../../constants";
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
	handleElementClick()
	{
		/*
		 TODO
		 Show modal with new Form
		 */
		this.setState({detailOpen: {open: true}});
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
	handleAddElement(){
		this.setState({ addElement: { open: true }});
	}

	handleElementDelete(elementId)
	{
		this.props.dispatch(ElementsActions.del(elementId));
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

	render() {
		return (
			<tr>
				<td>
					<a onClick={() => this.handleElementClick()}>
						{this.props.element.description}
					</a>
					<ModalWrapper title="Element Detail" open={this.state.detailOpen}>
						<ElementDetail element={this.props.element}/>
					</ModalWrapper>
				</td>
				<td>
					{ElementType.to_text[this.props.element.type]}
				</td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Edit Element" open={this.state.editOpen}  >
						<ElementEdit element={this.props.element} closeModal={this.closeModal.bind(this)} />
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
	constructor(props) {
		super(props);
		this.rows = [];
	}

	render(){
		const formId = this.props.page.formId;
		if (this.props.elements.reload) {
			this.props.dispatch(ElementsActions.fetchAll(formId));
		}
		this.rows = [];
		const elements = this.props.elements.elements;
		elements.forEach( (element) => {
			this.rows.push(<ElementListRow key={element.id} element={element} />)
		});

		return (
			<div className="row" id="forms">
				<table className="table table-striped row">
					<thead>
					<tr>
						<th>Element description</th>
						<th>Element type</th>
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