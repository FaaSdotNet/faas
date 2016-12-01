import {combineReducers} from 'redux';
import {routerStateReducer} from 'redux-router';
import user from './UserReducer';
import forms from './FormsReducer';
import projects from './ProjectsReducer';
import page from './PageReducer';
import elements from'./ElementsRefucer';

export default combineReducers({
	user,
	forms,
	projects,
	elements,
	page,
	router: routerStateReducer
});