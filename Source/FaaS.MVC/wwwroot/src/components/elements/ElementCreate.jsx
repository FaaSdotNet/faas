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
        this.handleRangeChange = this.handleRangeChange.bind(this); 

        this.state = {};
        this.state.description = "";
        this.state.options = "";
        this.state.type = "0";
        this.state.required = "";
        this.state.range = {min: "0", max: "100"};

        this.state.displayStyle = "none";
    }

    handleDescriptionChange(event) {
        this.setState({
            description: event.target.value,
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

    handleOptionsChange(event) {
        const id = event.target.id.substr(5);

        let options = JSON.parse(this.state.options);
        options[id] = event.target.value;

        this.setState({
            options: JSON.stringify(options),
        });
    }

    handleTypeChange(event) {
        if (event.target.value != "0" && event.target.value != "2") {
            this.state.options = "";
        }

        this.setState({
            type: event.target.value
        });
    }

    handleRequiredChange(event) {
        this.setState({
            required: event.target.checked
        });
    }

    handleSubmit()
	{
		const description = this.state.description;
        var options;
        if (this.state.type != "3") {
		    options = this.state.options;
        } else {
            options = JSON.stringify(this.state.range);
        }
		const type = this.state.type;
		const required = this.state.required;

        if (description.trim() == "")
		{
			this.setState({displayStyle: "inline"});
			return;
		}

		const payload = {
			Description: description,
			Options: options,
			Type: type,
			Required: required,
			FormId: this.props.formId
		};

		this.props.dispatch(ElementsActions.create(null, payload));
        this.props.closeModal();
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
            options: JSON.stringify(options)
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
        
        this.setState({
            options: JSON.stringify(options)
        });
    }

    handleRangeChange(event){
        const id = event.target.id;
        var newRange = this.state.range;
        newRange[id] = event.target.value;
        this.setState({range: newRange});
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

        var optionsDiv;

        if (this.state.type != "3") {
            optionsDiv =
                <div>
                    <label htmlFor="options" className="col-md-5 control-label">
                        Options
                    </label>
                    <br />
                    <div className="col-md-offset-5">
                        {optionElements}
                        <a id="add" onClick={this.handleAdd}
                            href="javascript:void(0)">
                            <i id="i" className="glyphicon glyphicon-plus-sign"></i>
                        </a>
                    </div>
                </div>
        } else {
            optionsDiv =
                <div>
                    <label htmlFor="options" className="col-md-5 control-label">
                        Range
                    </label>
                    <div className="col-xs-6">
                        <span>Min: </span>
                        <input id={"min"} onChange={this.handleRangeChange}
                            type="number" className="form-horizontal" value={this.state.range.min} />
                        <br />
                        <span>Max: </span>
                        <input id={"max"} onChange={this.handleRangeChange}
                            type="number" className="form-horizontal" value={this.state.range.max} />
                        <br />
                    </div>
                </div>
        }
        return (
            <div className="form-horizontal">
                <label htmlFor="description" className="col-md-5 control-label">
                    Description
                    <span style={{color: "red", display: this.state.displayStyle}}><b> * (Required)</b></span>
                </label>
                <input ref="editDescription" type="text" id="Description"
                       onChange={this.handleDescriptionChange} className="form-control"
                       value={this.state.description} />


                {optionsDiv}
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
                    <option value="5">Text Area</option>
                </select>

                <label htmlFor="required" className="col-md-5 control-label">
                    Required
                </label>
                <input ref="editRequired" type="checkbox" id="Required"
                       onChange={this.handleRequiredChange} className="form-control" />

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