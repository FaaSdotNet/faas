import React, { Component } from "react";
import {connect} from "react-redux";
import {FormsActions} from "../../actions/formsActions";

@connect((store) => {
  return store;
})
export class FormCreateComponent extends Component {

    constructor(props) {
        super(props);
        
        this.handleSubmit = this.handleSubmit.bind(this);
		this.handleFormNameChange = this.handleFormNameChange.bind(this);
		this.handleDescriptionChange = this.handleDescriptionChange.bind(this);

		this.state = {};
		this.state.formName = "";
		this.state.description = "";
		this.state.created = "";

        this.state.displayStyle = "none";
    }

    handleFormNameChange(event)
	{
		this.setState({
			formName: event.target.value
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

    handleSubmit() {
        const formName = this.state.formName;
        const formDesc = this.state.description;

        if (formName.trim() == "")
		{
			this.setState({displayStyle: "inline"});
			return;
		}

        const payload = {
			FormName: formName,
			Description: formDesc,
			ProjectId: this.props.projectId
		};

        this.props.dispatch(FormsActions.create(payload));
        this.props.closeModal();
    }

    render() {
        return (
            <div className="form-horizontal">
                <label className="col-md-5 control-label">
					Form Name
                    <span style={{color: "red", display: this.state.displayStyle}}><b> * (Required)</b></span>
				</label>
				<input type="text" id="FormName"
					   onChange={this.handleFormNameChange} className="form-control"
					   value={this.state.formName}/>
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
                        onClick={this.handleSubmit}
                        value="Create" 
                        className="btn btn-primary col-md-offset-5"/>
            </div>
        );
    }
}

export default FormCreateComponent;