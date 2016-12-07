import {combineReducers} from 'redux';
import {routerStateReducer} from 'redux-router';
import user from './UserReducer';
import users from './UsersReducer';
import forms from './FormsReducer';
import projects from './ProjectsReducer';
import sessions from './SessionsReducer';
import page from './PageReducer';
import elements from'./ElementsReducer';

export default combineReducers({
	user,
	users,
	forms,
	projects,
	sessions,
	elements,
	page,
	router: routerStateReducer
});