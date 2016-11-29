import {combineReducers} from 'redux';
import {routerStateReducer} from 'redux-router';
import user from './UserReducer';
import forms from './FormsReducer';
import projects from './ProjectsReducer';
import page from './PageReducer';

export default combineReducers({
	user : user,
	forms: forms,
	projects: projects,
	page: page,
	router: routerStateReducer
});