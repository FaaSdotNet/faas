import React, { Component } from "react";
import {connect} from "react-redux";
import {ProjectsActions} from "../../actions/projectsActions";
import {ButtonDelete} from "../table/ButtonDelete";
import {ModalWrapper} from "../table/ModalWrapper";
import ProjectEdit from "./ProjectEdit";
import FormCreate from "../forms/FormCreate";
import {PagesActions} from "../../actions/pagesActions";
import {FormsActions} from "../../actions/formsActions";

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
        this.state = {
        	editOpen:  {open: false},
			addForm: {open: false},
			detailOpen: {open: false}
		};

		this.handleAddForm = this.handleAddForm.bind(this);
		this.handleProjectClick = this.handleProjectClick.bind(this);
		this.handleEditClick = this.handleEditClick.bind(this);
		this.handleDeleteProject = this.handleDeleteProject.bind(this);
    }


    /**
     * Will open form list for projectId
     * @param projectId Project ID
     */
    handleProjectClick(projectId)
    {
		/*
		 TODO
		 Show modal with new Form
		 */
		this.props.dispatch(FormsActions.reset());
		this.props.dispatch(PagesActions.setProject(projectId));
		document.location.href="/#/forms/";

	}

	/**
	 * Will open form list for projectId
	 * @param projectId Project ID
	 */
	handleEditClick()
	{
		this.setState({ editOpen: { open: true }});
	}



	/**
	 * Adds form to specified project
	 * @param projectId Project Id
	 */
    handleAddForm(projectId){
		this.setState({ addForm: { open: true }});


	}

    handleDeleteProject(projectId)
    {
    	console.log("[DELETE] Project: ", projectId);
		this.props.dispatch(ProjectsActions.del(projectId));
    }

    /**
     *
     * @returns {XML}
     */

    render(){
        return (
            <tr>
                <td>
                    <a onClick={() => this.handleProjectClick(this.props.project.id)}>
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
					<ModalWrapper title="Edit Project" open={this.state.addForm}  >
						<FormCreate project={this.props.project} />
					</ModalWrapper>
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Edit Project" open={this.state.editOpen}  >
						<ProjectEdit project={this.props.project} />
					</ModalWrapper>
				</td>
				<td>
               		<ButtonDelete item={this.props.project} handleDelete={this.handleDeleteProject} />
				</td>
            </tr>
        );
    }
}


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

    render(){
		let userId = localStorage.getItem('userId');
		if(this.props.projects.reload) {
			this.props.dispatch(ProjectsActions.fetchAll(userId));
		}
		this.rows = [];
		const projects = this.props.projects.projects;
		console.log("Projects: ", projects);
		projects.forEach( (project) => {
			this.rows.push(<ProjectListRow key={project.id} project={project} />)
		});

        return (
            <div className="row" id="projects-list">
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