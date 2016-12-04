import React, {Component} from "react";
import MyInput from "../form/MyInput";
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
	}

	handleSubmit()
	{
		const projectName = this.refs.projectName.state.value;
		const projectDesc = this.refs.projectDescription.state.value;

		const payload = {

			ProjectName: projectName,
			Description: projectDesc
		};

		this.props.dispatch(ProjectActions.create(this.props.user, payload));

		/*const result = fetch('/api/v1.0/projects', {
		 method: 'POST',
		 headers: {
		 'Content-Type': 'application/json',
		 },
		 credentials: "same-origin",
		 body: JSON.stringify()
		 });

		 result.then((res) =>
		 {
		 if (res.ok) {
		 res.json()
		 .then((js) =>
		 {
		 console.log(js);
		 });
		 }
		 });*/
	}

	render()
	{
		return (
			<div className="form-horizontal">
				<MyInput ref="projectName" id="projectName" label="Project Name"/>
				<MyInput ref="projectDescription" id="projectDescription" label="Description"/>
				<br/>
				<input type="button"
					   id="submitButton"
					   onClick={ () => this.handleSubmit()}
					   value="Create"
					   className="btn btn-primary col-md-offset-5"/>

				<input type="button"
					   id="cancelButton"
					   onClick={this.handleCancel}
					   value="Cancel"
					   className="btn btn-default"/>
			</div>
		);
	}
}

export default ProjectCreateComponent;