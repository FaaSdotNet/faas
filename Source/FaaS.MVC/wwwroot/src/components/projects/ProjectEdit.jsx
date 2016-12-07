import React, {Component} from "react";
import {Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter} from 'react-modal-bootstrap';
import {connect} from "react-redux";
import {ProjectsActions} from "../../actions/projectsActions";

@connect( (store) => {
	return store;
})
export class ProjectEdit extends Component {

	constructor()
	{
		super();
		this.handleSubmit = this.handleSubmit.bind(this);
		this.handleProjectNameChange = this.handleProjectNameChange.bind(this);
		this.handleDescriptionChange = this.handleDescriptionChange.bind(this);

		this.state = {};
		this.state.projectName = "";
		this.state.description = "";
		this.state.created = "";
	}

	handleProjectNameChange(event)
	{
		this.setState({
			projectName: event.target.value
		});
	}

	handleDescriptionChange(event)
	{
		this.setState({
			description: event.target.value
		});
	}

	componentWillMount()
	{
		const result = fetch('/api/v1.0/projects/' + this.props.project.id,
			{
				method: "GET",
				credentials: "same-origin",
				headers: {
					'Content-Type': 'application/json'
				}
			});

		result.then((res) =>
		{
			if (res.ok) {
				res.json()
					.then((js) =>
					{
						this.setState(js);
					});
			}
		});
	}

	handleSubmit(event)
	{
		const projectName = this.state.projectName;
		const description = this.state.description;

		const payload = {
			Id: this.props.project.id,
			projectName: projectName,
			Description: description,
			Created: this.state.created
		};

		this.props.dispatch(ProjectsActions.update(payload));
		this.props.closeModal();
	}

	render()
	{
		return (
			<div className="form-horizontal">
				<label htmlFor="projectName" className="col-md-5 control-label">
					Project Name
				</label>
				<input ref="editProjectName" type="text" id="ProjectName"
					   onChange={this.handleProjectNameChange} className="form-control"
					   value={this.state.projectName}/>
				<label htmlFor="description" className="col-md-5 control-label">
					Description
				</label>
				<input ref="editDescription" type="text" id="Description"
					   onChange={this.handleDescriptionChange} className="form-control"
					   value={this.state.description}/>

				<br/>
				<input type="button"
					   id="editButton"
					   onClick={this.handleSubmit}
					   value="Save"
					   className="btn btn-primary col-md-offset-5"/>

			</div>
		);
	}
}

export default ProjectEdit;