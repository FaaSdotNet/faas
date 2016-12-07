import React, { Component } from "react";
import MyInput from "../form/MyInput"
import MySubmit from "../form/MySubmit"
import {connect} from "react-redux";
import {FormsActions} from "../../actions/formsActions";

@connect((store) => {
  return store;
})
export class FormCreateComponent extends Component {

    constructor(props) {
        super(props);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit() {
        const formName = this.refs.formName.state.value;
        const formDesc = this.refs.formDescription.state.value;

        const payload = {
			FormName: formName,
			Description: formDesc,
			ProjectId: this.props.projectId
		};

        this.props.dispatch(FormsActions.create(this.props.project, payload));
        this.props.closeModal();
    }

    render() {
        return (
            <div className="form-horizontal">
                <MyInput ref="formName" id="formName" label="Form Name"/>
                <MyInput ref="formDescription" id="formDescription" label="Description"/>
                
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