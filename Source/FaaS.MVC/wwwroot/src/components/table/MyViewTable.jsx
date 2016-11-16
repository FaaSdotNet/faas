import React, { Component } from "react";

export class MyTableRow extends Component
{
    constructor(props) {
        super(props);
    }
    
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
                        document.location.href = "/#/login";
                    });
            }
        });
    }

    handleEdit(event) {
        document.location.href = "/#/" + this.props.name + "/edit/" + this.props.object.id;
    }
    
    render(){
        const obj = this.props.object;
        return(
            <tr>
                <td>
                    <a href={`/#/${this.props.name}/${obj.id}`}>
                        {obj[this.props.propName]}
                    </a>
                </td>
                <td>
                    <button onClick={() => {this.handleEdit()}}
                            type="button" className="btn btn-warning btn-md" role="button">Edit</button>
                </td>
                <td>
                    <button onClick={() => {if (confirm('Delete the item?')) {this.handleDelete()};}}
                            type="button" className="btn btn-danger btn-md" role="button">Delete</button>
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

export default MyViewTable;