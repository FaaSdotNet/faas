/**
 * Created by Wermington on 27.11.16.
 */

import {
	Pages,
	PagesLoc
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	selectedProject: null,
	selectedForm: null,
	page: PagesLoc.Dashboard
};

export default createReducer(initialState, {
	[Pages.PageSet]: (state, payload) => {
		return {...state, page: payload};
	},
	[Pages.FormSet]: (state, payload) => {
		return {...state, selectedForm: payload};
	},
	[Pages.ProjectSet]: (state, payload) => {
		return {...state, selectedProject: payload};
	}
});