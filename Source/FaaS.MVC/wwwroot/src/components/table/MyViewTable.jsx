import React, { Component } from "react";
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';
import {MyTableRow} from "./MyTableRow"

export class MyViewTable extends Component {

    constructor(props) {
        super(props);
        const obj = {};
        obj[this.props.name] = null;
        this.url_prefix = "/api/v1.0";
        this.get_url = `${this.url_prefix}/${this.props.url}`;

        this.state = obj;
    }

    componentWillMount() {
        const url = this.get_url;
        
        const result = fetch(url,
         {
             method: "GET",
             credentials: "same-origin",
             headers: {
                'Content-Type': "application/json"
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