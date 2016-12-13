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

		this.state.displayStyle = "none";
	}

	handleProjectNameChange(event)
	{
		this.setState({
			projectName: event.target.value
		});

		if (event.target.value.trim() == "")
		{
			this.setState({displayStyle: "inline"});
		}
		else
		{
			this.setState({displayStyle: "none"});
		}
	}

	handleDescriptionChange(event)
	{
		this.setState({
			description: event.target.value
		});
	}

	componentWillMount()
	{
		this.setState(this.props.project);
	}

	handleSubmit(event)
	{
		const projectName = this.state.projectName;
		const description = this.state.description;

		if (projectName.trim() == "")
		{
			return;
		}

		const payload = {
			Id: this.props.project.id,
			projectName: projectName,
			Description: description,
			Created: this.state.created
		};

		this.props.dispatch(ProjectsActions.update(payload, this.props.user.userId));
		this.props.closeModal();
	}

	render()
	{
		return (
			<div className="form-horizontal">
				<label htmlFor="projectName" className="col-md-5 control-label">
					Project Name
					<span style={{color: "red", display: this.state.displayStyle}}><b> * (Required)</b></span>
				</label>
				<input type="text" id="ProjectName"
					   onChange={this.handleProjectNameChange} className="form-control"
					   value={this.state.projectName}/>
				<br/>
				<label htmlFor="description" className="col-md-5 control-label">
					Description
				</label>
				<input type="text" id="Description"
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