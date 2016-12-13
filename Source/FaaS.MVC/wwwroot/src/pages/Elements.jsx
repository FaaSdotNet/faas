import React, {Component} from "react";
import {ElementsListTable} from "../components/elements/ElementsListTable";
import {FormDetail} from "../components/forms/FormDetail";
import {ElementCreateComponent} from "../components/elements/ElementCreate";
import {ModalWrapper} from "../components/table/ModalWrapper";
import {connect} from "react-redux";

@connect((store) =>
{
	return store;
})
export class Elements extends Component {

	constructor(props)
	{
		super(props);
		this.handleAddElement = this.handleAddElement.bind(this);
		this.state = {
			createOpen: {open: false}
		}
	}

	handleAddElement()
	{
		this.setState({createOpen: {open: true}});
	}

	handleReturn()
	{
		document.location.href="/#/forms/";
	}

	closeModal()
	{
		this.setState({
			createOpen: false
		});
	}

	render()
	{
		if(!this.props.page.formId) {
			document.location.href="/#/forms";
		}
		return (
			<div>
				<div className="row">
					<FormDetail formId={this.props.page.formId} />
					<ElementsListTable />
				</div>

				<button onClick={() =>
				{
					this.handleAddElement()
				}}
						type="button"
						className="btn btn-primary btn-md" role="button">
					Add New Element
				</button>
				{'\u00A0'}
				<button onClick={() => { this.handleReturn() }}
						type="button"
						className="btn btn-default btn-md" role="button">
					Back to Project
				</button>
				<ModalWrapper title="Create element" open={this.state.createOpen}>
					<ElementCreateComponent formId={this.props.page.formId} closeModal={this.closeModal.bind(this)} />
				</ModalWrapper>
			</div>
		);
	}
}

export default Elements;