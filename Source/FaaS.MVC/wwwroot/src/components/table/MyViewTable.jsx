import React, { Component } from "react";

export class MyTableRow extends Component
{
    constructor(props) {
        super(props);
    }
  
    
    render(){
        const obj = this.props.object;
        return(
            <tr>
                <td>
                    <a href={`/api/v1.0/${this.props.name}/${obj.id}`}>
                        {obj[this.props.propName]}
                    </a>
                </td>
                <td>
                    <a href={`/api/v1.0/${this.props.name}/${obj.id}`}>
                        View
                    </a>
                </td>
                <td>
                    <a href={`/api/v1.0/${this.props.name}/${obj.id}`}>
                        Edit
                    </a>
                </td>
                <td>
                    <a href={`/api/v1.0/${this.props.name}/${obj.id}`}>
                        Delete
                    </a>
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
            if(res.ok) {
                res.json()
                    .then((js) => {
                        console.log(js);
                        const obj = {};
                        obj[this.props.name] = js;
                        this.setState(obj);
                    });
            }
            console.log(res);
        });
    }
    

    render() {
        const rows =[];
        const state = this.state[this.props.name];
        if (state) {
            state.forEach((obj) => {
                rows.push(<MyTableRow key={obj.id} name={this.props.name} object={obj} propName={this.props.propName} />);
                console.log(obj);
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
                            View
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