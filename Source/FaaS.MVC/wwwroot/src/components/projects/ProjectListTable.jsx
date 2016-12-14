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
		this.props.dispatch(ProjectsActions.del(projectId, this.props.user.userId));
    }

	closeModal()
	{
		this.setState({
			addForm: false,
			editOpen: false
		});
	}

	/**
     *
     * @returns {XML}
     */
    render(){
        return (
            <tr>
                <td>
                    <a class="details" onClick={() => this.handleProjectClick(this.props.project.id)}>
                        {this.props.project.projectName}
                    </a>
                </td>
                <td>
                    <button type="button" className="btn btn-default btn-md" onClick={ () => this.handleAddForm(this.props.project.id)}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-plus" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Add Form" open={this.state.addForm}  >
						<FormCreate projectId={this.props.project.id} closeModal={this.closeModal.bind(this)} />
					</ModalWrapper>
                </td>
				<td>
					<button type="button" className="btn btn-default btn-md" onClick={ () => this.handleEditClick()}>
						<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-edit" aria-hidden="true"/>
					</button>
					<ModalWrapper title="Edit Project" open={this.state.editOpen}  >
						<ProjectEdit project={this.props.project} closeModal={this.closeModal.bind(this)} />
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
export class ProjectListTable extends Component {
    
	/**
     * @param props
     * @property {Array} projects
     */
    constructor(props) {
        super(props);
		this.rows = [];
    }

    render(){
        let userId = this.props.user.userId;
		if(this.props.projects.reload) {
			this.props.dispatch(ProjectsActions.fetchAll(userId));
		}
		this.rows = [];
		const projects = this.props.projects.projects;

		projects.forEach( (project) => {
			this.rows.push(<ProjectListRow key={project.id} project={project} />)
		});

        return (
            <div className="row" id="projects-list">
                <table className="table table-striped row">
                    <thead>
						<tr>
							<th>Project name</th>
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