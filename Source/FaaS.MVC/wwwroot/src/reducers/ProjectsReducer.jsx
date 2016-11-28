/**
 * Created by Wermington on 27.11.16.
 */

import {
	PROJECTS_SET,
	PROJECTS_UNSET,
	PROJECTS_CREATE_SUCC,
	PROJECTS_FETCH_SUCC,
	PROJECTS_ANY_FAIL,
	PROJECTS_DELETE_SUCC,
	PROJECTS_UPDATE_SUCC,
	PROJECTS_GET_SUCC
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	currentProject: null,
	projects: {},
	statusText: null
};

export default createReducer(initialState, {
	[PROJECTS_SET]: (state, payload) =>
	{
		return {...state, currentProject: payload};
	},
	[PROJECTS_UNSET]: (state, payload) => {
		return {...state, currentProject: null};

	},
	[PROJECTS_FETCH_SUCC]: (state, payload) => {
		return {...state, projects: payload};
	},
	[PROJECTS_ANY_FAIL]: (state, payload) => {
		return {...state, statusText: "[FAIL]: "+ payload};
	},
	[PROJECTS_CREATE_SUCC]: (state, payload) => {
		return {...state, projects: Object.assign({}, state.projects, payload), currentProject: payload.Id};
	},
	[PROJECTS_DELETE_SUCC]: (state, payload) => {
		return state;
	},
	[PROJECTS_GET_SUCC]: (state, payload) => {
		return {...state, currentProject: payload.Id}
	},
	[PROJECTS_UPDATE_SUCC]: (state, payload) => {
		return {...state, projects: Object.assign({}, state.projects, payload), currentProject: payload.Id};
	}
});

