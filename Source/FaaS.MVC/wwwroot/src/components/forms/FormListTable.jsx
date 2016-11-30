import React, { Component } from "react";


/**
 *
 */
export class FormListRow extends Component{

    /**
     * Creates List table row
     * @param props
     * @property {Object} form
     */
    constructor(props){
        super(props);
    }

    /**
     * Will open form list for formId
     * @param formId form ID
     */
    handleFormClick(formId)
    {

    }

    /**
     * Adds form to specified project
     * @param formId form Id
     */
    handleAddElement(formId)
    {

    }

    handleDeleteForm(formId)
    {

    }

    /**
     *
     * @returns {XML}
     */

    render(){
        return (
            <tr>
                <td>
                    <a href="#" onClick={() => this.handleFormClick(this.props.form.id)}>
                        {this.props.project.name}
                    </a>
                </td>
                <td>
                    {this.props.project.numForms}
                </td>
                <td>
                    <button onClick={ () => this.handleAddElement(this.props.form.id)}>
                        Add Form
                    </button>
                </td>
                <td>
                    <button onClick={ () => this.handleDeleteForm(this.props.form.id)}>

                    </button>
                </td>
            </tr>
        );
    }

}


/**
 *
 */
export class ProjectListTable extends Component{
    /**
     * @param props
     * @property {Array} forms
     */
    constructor(props){
        super(props);
        this.rows = [];
    }

    componentWillUpdate() {
        this.rows = [];
        this.props.forms.forEach( (form) => {
            this.rows.push(<FormListRow form={form} />)
        });
    }

    render(){
        return (
            <div className="row" id="forms">
                <h1>
                    Forms
                </h1>
                <table>
                    <thead>
                    <th>Form name</th>
                    <th>Number of elements</th>
                    <th>Add element</th>
                    <th>View</th>
                    <th>Edit</th>
                    <th>Delete</th>
                    </thead>
                    <tbody>
                    {this.rows}
                    </tbody>
                </table>
            </div>
        );
    }

}

export default FormListTable;