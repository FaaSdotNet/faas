import React, { Component } from "react";
import MyViewTable from "../table/MyViewTable"


export class ElementListComponent extends Component {

    constructor(props) {
        super(props);
    }

    handleAdd(event) {
        document.location.href = `/#/elements/create/${this.props.formid}`;
    }

    render() {
        const url = `elements/?${this.props.formid}`;
        return (
            <div className="row">
                <MyViewTable url={url} name="elements" propName="description"/>
                <button onClick={() => {this.handleAdd()}}
                            type="button" className="btn btn-primary btn-md" role="button">Add New Element</button>
            </div>
        );
    }
}

export default ElementListComponent;