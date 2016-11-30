import React, { Component } from "react";


/**
 *
 */
export class ProjectListRow extends Component{

    /**
     * Creates List table row
     * @param props
     * @property {Object} project
     */
    constructor(props){
        super(props);
    }

    /**
     * Will open form list for projectId
     * @param projectId Project ID
     */
    handleFormClick(projectId)
    {

    }

    /**
     * Adds form to specified project
     * @param projectId Project Id
     */
    handleAddForm(projectId)
    {

    }

    handleDeleteProject(projectId)
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
                    <a href="#" onClick={() => this.handleFormClick(this.props.project.id)}>
                        {this.props.project.name}
                    </a>
                </td>
                <td>
                    {this.props.project.numForms}
                </td>
                <td>
                    <button onClick={ () => this.handleAddForm(this.props.project.id)}>
                        Add Form
                    </button>
                </td>
                <td>
                    <button onClick={ () => this.handleDeleteProject(this.props.project.id)}>

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
     * @property {Array} projects
     */
    constructor(props){
        super(props);
        this.rows = [];
    }

    componentWillUpdate() {
        this.rows = [];
        this.props.projects.forEach( (project) => {
            this.rows.push(<ProjectListRow project={project} />)
        });
    }

    render(){
        return (
            <div className="row" id="projects">
                <h1>
                    Projects
                </h1>
                <table>
                    <thead>
                    <th>Project name</th>
                    <th>Number of forms</th>
                    <th>Add form</th>
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

export default ProjectListTable;