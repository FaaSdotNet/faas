/**
 * Created by Wermington on 27.11.16.
 */

import {
	Projects
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	projects: {},
	statusText: null
};

export default createReducer(initialState, {
	[Projects.FetchSucc]: (state, payload) => {
		return {...state, projects: payload};
	},
	[Projects.Fail]: (state, payload) => {
		return {...state, statusText: "[FAIL]: "+ payload};
	},
	[Projects.CreateSucc]: (state, payload) => {
		return {...state, projects: Object.assign({}, state.projects, payload)};
	},
	[Projects.DeleteSucc]: (state, payload) => {
		return state;
	},
	[Projects.GetSucc]: (state, payload) => {
		return {...state, currentProject: payload.Id}
	},
	[Projects.UpdateSucc]: (state, payload) => {
		return {...state, projects: Object.assign({}, state.projects, payload)};
	}
});

