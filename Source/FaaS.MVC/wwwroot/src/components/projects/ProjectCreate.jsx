import React, {Component} from "react";
import ProjectActions from "../../actions/projectsActions";
import {connect} from "react-redux";

@connect((store) =>
{
	return store;
})
export class ProjectCreateComponent extends Component {

	constructor(props)
	{
		super(props);
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

	handleSubmit()
	{
		const projectName = this.state.projectName;
		const projectDesc = this.state.description;

		if (projectName.trim() == "")
		{
			this.setState({displayStyle: "inline"});
			return;
		}

		const payload = {
			ProjectName: projectName,
			Description: projectDesc
		};

		this.props.dispatch(ProjectActions.create(this.props.user.userId, payload));
		this.props.closeModal();
	}

	render()
	{
		return (
			<div className="form-horizontal">
				<label className="col-md-5 control-label">
					Project Name
					<span style={{color: "red", display: this.state.displayStyle}}><b> * (Required)</b></span>
				</label>
				<input type="text" id="ProjectName"
					   onChange={this.handleProjectNameChange} className="form-control"
					   value={this.state.projectName}/>
				<br/>
				<label className="col-md-5 control-label">
					Description
				</label>
				<input type="text" id="Description"
					   onChange={this.handleDescriptionChange} className="form-control"
					   value={this.state.description}/>

				<br/>
				<input type="button"
					   id="submitButton"
					   onClick={ () => this.handleSubmit()}
					   value="Create"
					   className="btn btn-primary col-md-offset-5"/>
			</div>
		);
	}
}

export default ProjectCreateComponent;