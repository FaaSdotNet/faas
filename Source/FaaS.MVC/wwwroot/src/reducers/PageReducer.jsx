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

export function pageReducer(state, action){
	state = state || initialState;
	const payload = action.payload;
	switch (action.type){
		case Pages.PageSet:
			return {...state, page: payload};
		case Pages.FormSet:
			return {...state, selectedForm: payload};
		case Pages.ProjectSet:
			return {...state, selectedProject: payload};
	}

	return state;
}

export default pageReducer;