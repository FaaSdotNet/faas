import React, {Component} from "react";
import {FormsListTable} from "../components/forms/FormsListTable";
import FormCreate from "../components/forms/FormCreate";
import {ModalWrapper} from "../components/table/ModalWrapper";
import {connect} from "react-redux";

@connect((store) => {
	return store;
})
export class Forms extends Component {

	constructor(props)
	{
		super(props);
		console.log("[FORMS] Props: ", this.props);
		this.handleAdd = this.handleAdd.bind(this);
		this.state = {
			createOpen: {open: false}
		}
	}

	handleAdd()
	{
		this.setState({createOpen: {open: true}});
	}

	render()
	{
		return (
			<div>
				<div className="row">

					<FormsListTable projectId={this.props.page.projectId} />
				</div>
				<button onClick={() =>
				{
					this.handleAdd()
				}}
						type="button"
						className="btn btn-primary btn-md" role="button">
					Add New Form
				</button>
				<ModalWrapper title="Create form" open={this.state.createOpen}>
					<FormCreate project={this.props.page.projectId} />
				</ModalWrapper>
			</div>
		);
	}
}

export default Forms;