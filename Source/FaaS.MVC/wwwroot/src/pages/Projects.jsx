import React, {Component} from "react";
import {ProjectListTable} from "../components/projects/ProjectListTable";
import ProjectCreate from "../components/projects/ProjectCreate";
import {ModalWrapper} from "../components/table/ModalWrapper";
import {connect} from "react-redux";
@connect((store) => {
	return store;
})
export class Projects extends Component {

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

	closeModal()
	{
		this.setState({
			createOpen: false
		});
	}

	render()
	{

		if(!this.props.user.userId){
			document.location.href="/";
		}

		return (
			<div>
				<h1>
					Projects
				</h1>
				<div className="row">

					<ProjectListTable />
				</div>
				<button onClick={() =>
				{
					this.handleAdd()
				}}
						type="button"
						className="btn btn-primary btn-md" role="button">
					Add New Project
				</button>
				<ModalWrapper title="Create project" open={this.state.createOpen}>
					<ProjectCreate closeModal={this.closeModal.bind(this)} />
				</ModalWrapper>
			</div>
		);
	}
}

export default Projects;