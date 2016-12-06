import React, {Component} from "react";
import {ElementsListTable} from "../components/elements/ElementsListTable";
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

	render()
	{
		if(!this.props.page.formId) {
			document.location.href="/#/forms";
		}
		return (
			<div>
				<h1>
					Elements
				</h1>
				<div className="row">

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
				<ModalWrapper title="Create element" open={this.state.createOpen}>
					<ElementCreateComponent form={this.props.page.formId}/>
				</ModalWrapper>
			</div>
		);
	}
}

export default Elements;