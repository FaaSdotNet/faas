import React, { Component } from "react";
import {connect} from "react-redux";
import {ProjectsActions} from "../../actions/projectsActions"

@connect((store) => {
	return store;
})
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
    handleProjectClick(projectId)
    {
		ProjectsActions.get(projectId)
		/*
		 TODO
		 Show modal with new Form
		 */
    }

	/**
	 * Will open form list for projectId
	 * @param projectId Project ID
	 */
	handleEditClick(projectId)
	{

	}


	/**
     * Adds form to specified project
     * @param projectId Project Id
     */
    handleAddProject()
    {
		/*
		 TODO
		 Show modal with new Project
		 */
    }

	/**
	 * Adds form to specified project
	 * @param projectId Project Id
	 */
    handleAddForm(projectId){
    	/*
    	TODO
    	 Show modal with new Form
    	*/

	}

    handleDeleteProject(projectId)
    {
		ProjectsActions.del(projectId);
    }

    /**
     *
     * @returns {XML}
     */

    render(){
        return (
            <tr>
                <td>
                    <a href="#" onClick={() => this.handleProjectClick(this.props.project.id)}>
                        {this.props.project.projectName}
                    </a>
                </td>
                <td>
                    {this.props.project.numForms}
                </td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleAddForm(this.props.project.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-plus" aria-hidden="true"/>

					</button>
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick(this.props.project.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
				</td>
                <td>
                    <button type="button" className="btn btn-default btn-md"	 onClick={ () => this.handleDeleteProject(this.props.project.id)}>
                    <span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-trash" aria-hidden="true"/>
					</button>
                </td>

            </tr>
        );
    }

}


/**
 *
 */

@connect((store) => {
	return store;
})
export class ProjectListTable extends Component{
    /**
     * @param props
     * @property {Array} projects
     */
    constructor(props){
        super(props);
		console.log("Project List Table: ", this.props);
		this.rows = [];
    }

	componentWillMount(){
    	let userId = this.props.user.userId;
    	userId = localStorage.getItem('userId');
    	console.log('User id: ', userId);
		this.props.dispatch(ProjectsActions.fetchAll(userId));
	}


    render(){

		this.rows = [];
		const projects = this.props.projects.projects;
		console.log("Projects: ", projects);
		projects.forEach( (project) => {
			this.rows.push(<ProjectListRow key={project.id} project={project} />)
		});

        return (
            <div className="row" id="projects">
                <h1>
                    Projects
                </h1>
                <table className="table table-striped row">
                    <thead>
						<tr>
							<th>Project name</th>
							<th>Number of forms</th>
							<th>Add form</th>
							<th>Edit</th>
							<th>Delete</th>
						</tr>

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