import React, {Component} from "react";
import {FormsListTable} from "../components/forms/FormsListTable";
import FormCreate from "../components/forms/FormCreate";
import ProjectDetail from "../components/projects/ProjectDetail";
import {ModalWrapper} from "../components/table/ModalWrapper";
import {connect} from "react-redux";

@connect((store) => {
	return store;
})
export class Forms extends Component {

	constructor(props)
	{
		super(props);
		this.handleAdd = this.handleAdd.bind(this);
		this.state = {
			createOpen: {open: false}
		}
	}

	handleAdd()
	{
		this.setState({createOpen: {open: true}});
	}

	handleReturn()
	{
		document.location.href="/#/projects/";
	}

	closeModal()
	{
		this.setState({
			createOpen: false
		});
	}

	render()
	{
		if(!this.props.page.projectId) {
			document.location.href="/#/projects";
		}

		return (
			<div>
				<div className="row">

					<div>
						<ProjectDetail projectId={this.props.page.projectId}/>
					</div>

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
				<button onClick={() => { this.handleReturn() }}
						type="button"
						className="btn btn-default btn-md" role="button">
					Back to Projects
				</button>
				<ModalWrapper title="Create form" open={this.state.createOpen}>
					<FormCreate projectId={this.props.page.projectId} closeModal={this.closeModal.bind(this)} />
				</ModalWrapper>
			</div>
		);
	}
}

export default Forms;