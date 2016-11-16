import React, { Component } from "react";

class ElementEdit extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleOptionsChange = this.handleOptionsChange.bind(this);
        this.handleTypeChange = this.handleTypeChange.bind(this);
        this.handleRequiredChange = this.handleRequiredChange.bind(this);

        this.state = {};
        this.state.description = "";
        this.state.options = "";
        this.state.type = "";
        this.state.required = "";
        this.state.optionsAttributes = {};
    }

    handleDescriptionChange(event) {
        this.setState({description: event.target.value,
            options: this.state.options,
            type: this.state.type,
            required: this.state.required,
            optionsAttributes: this.state.optionsAttributes});
    }

    handleOptionsChange(event) {
        this.setState({description: this.state.description,
            options: event.target.value,
            type: this.state.type,
            required: this.state.required,
            optionsAttributes: this.state.optionsAttributes});
    }

    handleTypeChange(event) {
        this.setState({description: this.state.description,
            options: this.state.options,
            type: event.target.value,
            required: this.state.required,
            optionsAttributes: this.state.optionsAttributes});

        if (this.state.type != "radio" && this.state.type != "checkbox")
        {
            this.state.optionsAttributes['disabled'] = "disabled";
        }
        else
        {
            this.state.optionsAttributes['disabled'] = "";
        }
    }

    handleRequiredChange(event) {
        this.setState({description: this.state.description,
            options: this.state.options,
            type: this.state.type,
            required: event.target.value,
            optionsAttributes: this.state.optionsAttributes});
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/elements/' + this.props.params.elementid,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json'
            }
        });

        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        console.log(js);
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        const description = this.state.description;
        const options = this.state.options;
        const type = this.state.type;
        const required = this.state.required;
        var result = fetch('/api/v1.0/elements', {
            method: 'PUT',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Id: this.props.params.elementid,
                Description: description,
                Options: options,
                Type: type,
                Required: required
            })
        });
        
        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/login";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>Edit Element</h4>

                <label htmlFor="description" className="col-md-5 control-label">
                    Description
                </label>
                <input ref="editDescription" type="text" id="Description"
                       onChange={this.handleDescriptionChange} className="form-control"
                       value={this.state.description} />

                <label htmlFor="options" className="col-md-5 control-label">
                    Options
                </label>
                <input {...this.state.optionsAttributes} ref="editOptions" type="text" id="Options"
                       onChange={this.handleOptionsChange} className="form-control"
                       value={this.state.options} />
                
                <label htmlFor="type" className="col-md-5 control-label">
                    Type
                </label>
                <select className="form-control" ref="editType"
                        value={this.state.type} id="Type" onChange={this.handleTypeChange}>
                    <option value="checkbox">Check Box</option>
                    <option value="date">Date</option>
                    <option value="radio">Radio</option>
                    <option value="range">Range</option>
                    <option value="text">Text</option>
                </select>

                <label htmlFor="required" className="col-md-5 control-label">
                    Required
                </label>
                <input ref="editRequired" type="checkbox" id="Required"
                       onChange={this.handleRequiredChange} className="form-control"
                       value={this.state.required} />

                <input type="button" 
                        id="editButton"
                        onClick={this.handleSubmit}
                        value="Save" 
                        className="btn btn-default"/>
            </div>
        );
    }
}

export default ElementEdit;