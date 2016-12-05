import React, {Component} from "react";
import {FormsActions} from "../../actions/formsActions";
import {connect} from "react-redux";

@connect( (store) => {
	return store;
})
class FormEdit extends Component {

	constructor()
	{
		super();
		this.handleSubmit = this.handleSubmit.bind(this);
		this.handleFormNameChange = this.handleFormNameChange.bind(this);
		this.handleDescriptionChange = this.handleDescriptionChange.bind(this);

		this.state = {};
		this.state.formName = "";
		this.state.description = "";
		this.state.created = "";
	}

	handleFormNameChange(event)
	{
		this.setState({
			formName: event.target.value,
			description: this.state.description,
			created: this.state.created
		});
	}

	handleDescriptionChange(event)
	{
		this.setState({
			formName: this.state.formName,
			description: event.target.value,
			created: this.state.created
		});
	}

	componentWillMount()
	{
		const result = fetch('/api/v1.0/forms/' + this.props.form.id,
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
		const formName = this.state.formName;
		const description = this.state.description;
		const payload = {
			Id: this.props.form.id,
			formName: formName,
			Description: description,
			Created: this.state.created
		};

		this.props.dispatch(FormsActions.update(payload));
	}

	render()
	{
		return (
			<div className="form-horizontal">
				<h4>Edit Form</h4>
				<label htmlFor="formName" className="col-md-5 control-label">
					Form Name
				</label>
				<input ref="editFormName" type="text" id="FormName"
					   onChange={this.handleFormNameChange} className="form-control"
					   value={this.state.formName}/>
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

				<input type="button"
					   id="cancelButton"
					   onClick={this.handleCancel}
					   value="Cancel"
					   className="btn btn-default"/>
			</div>
		);
	}
}

export default FormEdit;