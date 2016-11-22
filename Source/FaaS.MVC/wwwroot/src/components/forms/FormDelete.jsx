import React, { Component } from "react";

class FormDelete extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);

        this.state = {};
        this.state.formName = "";
        this.state.description = "";
        this.state.created = "";
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/forms/' + this.props.params.formid,
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
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        var result = fetch('/api/v1.0/forms/' + this.props.params.formid,
        {
            method: 'DELETE',
            credentials: "same-origin",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            }
        });
        result.then( (res) =>  {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href ="/#/dashboard";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>Delete Form</h4>
                <p>Delete form with Name: {this.state.formName} ?</p>

                <input type="button" 
                    id="deleteButton"
                    onClick={this.handleSubmit}
                    value="Delete" 
                    className="btn btn-default"/>
            </div>
        );
    }
}

export default FormDelete;