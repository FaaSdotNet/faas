import React, { Component } from "react";
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';

export class MyTableRow extends Component
{
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
        var result = fetch('/api/v1.0/' + this.props.name + '/' + this.props.object.id,
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
                        document.location.href = "/#/" + this.props.name;
                    });
            }
        });
    }

    handleEdit(event) {
        document.location.href = "/#/" + this.props.name + "/edit/" + this.props.object.id;
    }
    
    render(){
        const obj = this.props.object;
        var itemGroup = this.props.name.substr(0, this.props.name.length - 1);
        return(
            <tr>
                <td>
                    <a href={`/#/${this.props.name}/${obj.id}`}>
                        {obj[this.props.propName]}
                    </a>
                </td>
                <td>
                    <button onClick={() => {this.handleEdit()}}
                            type="button" className="btn btn-default btn-md">
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"></span>
                    </button>
                </td>
                <td>
                    <button onClick={this.openModal}
                            type="button" className="btn btn-default btn-md">
                        <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-trash" aria-hidden="true"></span>
                    </button>
                    <Modal isOpen={this.state.isOpen} onRequestHide={this.hideModal}>
                        <ModalHeader>
                            <ModalClose onClick={this.hideModal} />
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

export class MyViewTable extends Component {

    constructor(props) {
        super(props);
        const obj = {};
        obj[this.props.name] = null;

        this.state = obj;
    }

    componentWillMount() {
         const result = fetch('/api/v1.0/' + this.props.name,
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
                        const obj = {};
                        obj[this.props.name] = js;
                        this.setState(obj);
                    });
            }
        });
    }

    render() {
        const rows =[];
        const state = this.state[this.props.name];
        if (state) {
            state.forEach((obj) => {
                rows.push(<MyTableRow key={obj.id} name={this.props.name} object={obj} propName={this.props.propName} />);
            });
        }
        
        return (
            <table className="table table-striped row">
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Edit
                        </th>
                        <th>
                            Delete
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
        );
    }
}
//onClick={() => {if (confirm('Delete the item?')) {this.handleDelete()};}}
export default MyViewTable;