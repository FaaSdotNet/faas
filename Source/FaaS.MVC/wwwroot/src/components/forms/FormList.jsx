import React, { Component } from "react";
import MyViewTable from "../table/MyViewTable"


export class FormListComponent extends Component {

    constructor(props) {
        super(props);
    }

    handleAdd(event) {
        document.location.href = "/#/forms/create";
    }

    render() {
        return (
            <div className="row">
                <MyViewTable name="forms" propName="formName" />
                <button onClick={() => {this.handleAdd()}}
                            type="button" className="btn btn-primary btn-md" role="button">Add New Form</button>
            </div>
        );
    }
}

export default FormListComponent;