import React, { Component } from "react";
import {connect} from "react-redux";
import {ElementsActions} from "../../actions/elementsActions";


@connect((store)=> {
	return store;
})
export class ElementCreateComponent extends Component {

    constructor(props) {
        super(props);

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleOptionsChange = this.handleOptionsChange.bind(this);
        this.handleTypeChange = this.handleTypeChange.bind(this);
        this.handleRequiredChange = this.handleRequiredChange.bind(this);
        this.handleRemove = this.handleRemove.bind(this);
        this.handleAdd = this.handleAdd.bind(this);

        this.state = {};
        this.state.description = "";
        this.state.options = "";
        this.state.type = "0";
        this.state.required = "";
    }

    handleDescriptionChange(event) {
        this.setState({description: event.target.value,
            options: this.state.options,
            type: this.state.type,
            required: this.state.required});
    }

    handleOptionsChange(event) {
        const id = event.target.id.substr(5);

        let options = JSON.parse(this.state.options);
        options[id] = event.target.value;

        this.setState({description: this.state.description,
            options: JSON.stringify(options),
            type: this.state.type,
            required: this.state.required});
    }

    handleTypeChange(event) {
        if (event.target.value != "0" && event.target.value != "2")
        {
            this.state.options = "";
        }

        this.setState({description: this.state.description,
            options: this.state.options,
            type: event.target.value,
            required: this.state.required});
    }

    handleRequiredChange(event) {
        this.setState({description: this.state.description,
            options: this.state.options,
            type: this.state.type,
            required: event.target.value});
    }

    handleSubmit()
	{
		const description = this.state.description;
		const options = this.state.options;
		const type = this.state.type;
		const required = this.state.required;

		const payload = {
			Description: description,
			Options: options,
			Type: type,
			Required: required,
			FormId: this.props.page.formId
		};

		this.props.dispatch(ElementsActions.create(null, payload));

	}

    handleAdd(event) {
        if (this.state.type != "0" && this.state.type != "2")
        {
            return;
        }
        let options = {};
        if (this.state.options)
        {
            options = JSON.parse(this.state.options);
            const optionsCount = Object.keys(options).length + 1;

            options[optionsCount] = "";
        }
        else
        {
            options[1] = "";
        }
        this.setState({
            description: this.state.description,
            options: JSON.stringify(options),
            type: this.state.type,
            required: this.state.required
        });
    }

    handleRemove(event) {
        const id = event.target.id.substr(1);

        let options = JSON.parse(this.state.options);
        delete options[id];
        const optionsArray = Object.keys(options).map(key => options[key]);
        options = {};
        for (let i = 1; i <= optionsArray.length; i++)
        {
            options[i] = optionsArray[i-1];
        }
        
        this.setState({description: this.state.description,
            options: JSON.stringify(options),
            type: this.state.type,
            required: this.state.required});
    }

    render() {
        let optionElements = [];
        if (this.state.options)
        {
            const options = JSON.parse(this.state.options);
            const optionsCount = Object.keys(options).length;

            for (let i = 1; i <= optionsCount; i++)
            {
                optionElements.push(<div key={"option" + i} id={"option" + i}>
                    <span>Option {i}: </span>
                    <input id={"input" + i} onChange={this.handleOptionsChange} 
                            type="text" className="form-horizontal" value={options[i]}/>
                    <a id={"remove" + i} onClick={this.handleRemove} 
                        href="javascript:void(0)"> <i id={"i" + i} className="glyphicon glyphicon-remove"></i></a>
                    <br/>
                </div>);
            }
        }
        return (
            <div className="form-horizontal">
                <h4>Create New Element</h4>

                <label htmlFor="description" className="col-md-5 control-label">
                    Description
                </label>
                <input ref="editDescription" type="text" id="Description"
                       onChange={this.handleDescriptionChange} className="form-control"
                       value={this.state.description} />

                <label htmlFor="options" className="col-md-5 control-label">
                        Options
                </label>
                <br/>
                <div className="col-md-offset-5">
                    {optionElements}
                    <a id="add" onClick={this.handleAdd}
                        href="javascript:void(0)">
                        <i id="i" className="glyphicon glyphicon-plus-sign"></i>
                    </a>
                </div>
                <br/>
                
                <label htmlFor="type" className="col-md-5 control-label">
                    Type
                </label>
                <select className="form-control" ref="editType"
                        value={this.state.type} id="Type" onChange={this.handleTypeChange}>
                    <option value="0">Check Box</option>
                    <option value="1">Date</option>
                    <option value="2">Radio</option>
                    <option value="3">Range</option>
                    <option value="4">Text</option>
                </select>

                <label htmlFor="required" className="col-md-5 control-label">
                    Required
                </label>
                <input ref="editRequired" type="checkbox" id="Required"
                       onChange={this.handleRequiredChange} className="form-control"
                       value={this.state.required} />

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

export default ElementCreateComponent;