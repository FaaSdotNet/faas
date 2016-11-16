import React, { Component } from "react";

class ElementDelete extends Component {

    constructor() {
        super();
        this.handleSubmit = this.handleSubmit.bind(this);

        this.state = {};
        this.state.description = "";
        this.state.options = "";
        this.state.type = "";
        this.state.required = "";
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
                        this.setState(js);
                    });
            }
        });
    }

    handleSubmit(event) {
        var result = fetch('/api/v1.0/elements/' + this.props.params.elementid,
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
                        document.location.href ="/#/login";
                    });
            }
        });
    }

    render() {
        return (
            <div className="form-horizontal">
                <h4>Delete Element</h4>
                <p>Delete element with description: {this.state.description} ?</p>

                <input type="button" 
                    id="deleteButton"
                    onClick={this.handleSubmit}
                    value="Delete" 
                    className="btn btn-default"/>
            </div>
        );
    }
}

export default ElementDelete;