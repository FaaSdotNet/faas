import React, { Component } from "react";
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';


export class MyTableRow extends Component {
    constructor(props) {
        super(props);
        this.openModal = this.openModal.bind(this);
        this.hideModal = this.hideModal.bind(this);

        this.state = {
            isOpen: false
        };
    }

    openModal() {
        this.setState({
            isOpen: true
        });
    };

    hideModal() {
        this.setState({
            isOpen: false
        });
    };

    handleDelete(event) {
        const url = `/api/v1.0/${this.props.name}/${this.props.object.id}`;
        const request = {
            method: 'DELETE',
            credentials: "same-origin",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
            }
        };
        const result = fetch(url, request);

        result.then((res) => {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        document.location.href = "/#/dashboard";
                    });
            }
        });
    }

    handleEdit(event) {
        document.location.href = "/#/" + this.props.name + "/edit/" + this.props.object.id;
    }

    render() {
        const obj = this.props.object;
        const itemGroup = this.props.name.substr(0, this.props.name.length - 1);
        return(
            <tr>
                <td>
                    <a href={`/#/${this.props.name}/${obj.id}`}>
                        {obj[this.props.propName]}
                    </a>
                </td>
                <td>
                    <button onClick={() => { this.handleEdit() }}
                            type="button" className="btn btn-default btn-md">
                        <span style={{ fontSize: 1.5 + 'em' }} className="glyphicon glyphicon-edit" aria-hidden="true">
</span>
                    </button>
                </td>
                <td>
                    <button onClick={this.openModal} type="button" className="btn btn-default btn-md">
                        <span style={{ fontSize: 1.5 + 'em' }} className="glyphicon glyphicon-trash" aria-hidden="true"
></span>
                    </button>
                    <Modal isOpen={this.state.isOpen} onRequestHide={this.hideModal}>
                        <ModalHeader>
                            <ModalClose onClick={this.hideModal}/>
                            <ModalTitle>Confirm Delete</ModalTitle>
                        </ModalHeader>
                        <ModalBody>
                            <p>Delete {itemGroup}?</p>
                        </ModalBody>
                        <ModalFooter>
                            <button onClick={() => { this.handleDelete() } }
                                    className='btn btn-primary'>
                                Delete
                            </button>
                            <button className='btn btn-default' onClick={this.hideModal}>
                                Close
                            </button>
                        </ModalFooter>
                    </Modal>
                </td>
            </tr>
        );
    }

}