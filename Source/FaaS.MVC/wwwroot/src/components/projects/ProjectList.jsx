import React, {Component} from "react";
import {ProjectListTable} from "./ProjectListTable";
import ProjectCreate from "./ProjectCreate";
import {ModalWrapper} from "../table/ModalWrapper";

export class ProjectListComponent extends Component {

	constructor(props)
	{
		super(props);
		this.handleAdd = this.handleAdd.bind(this);
		this.state = {
			createOpen: false
		}
	}

	handleAdd()
	{
		this.setState({createOpen: true});
	}

	render()
	{
		return (
			<div>
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
					<ProjectCreate  />
				</ModalWrapper>
			</div>
		);
	}
}

export default ProjectListComponent;