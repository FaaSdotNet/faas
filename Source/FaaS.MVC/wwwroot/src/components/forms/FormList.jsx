import React, { Component } from "react";
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';
import MyViewTable from "../table/MyViewTable"

export class FormListComponent extends Component {

    constructor(props) {
        super(props);
        this.handleAdd = this.handleAdd.bind(this);
    }

    componentDidMount() {
        console.log(this.props);

    }

    handleAdd(event) {
        document.location.href = `/#/forms/create/${this.props.projectid}`;
                    }



    render() {
        const url = `forms/?projectId=${this.props.projectid}`;
        return (
            <div className="row">
                <MyViewTable url={url} name="forms" propName="formName" parent={this.props.projectid}/>
                <button onClick={() => { this.handleAdd() }}
                        type="button" className="btn btn-primary btn-md" role="button">Add New Form</button>
            </div>
        );
    }
}

export default FormListComponent;