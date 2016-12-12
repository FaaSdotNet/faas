/**
 * Created by Wermington on 27.11.16.
 */

import {
	Pages,
	PagesLoc
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	projectId: null,
	formId: null,
	elementId: null,
	page: PagesLoc.Dashboard
};

export function pageReducer(state, action){
	state = state || initialState;
	const payload = action.payload;
	switch (action.type) {
		case Pages.PageSet:
			return {...state, page: payload};
		case Pages.FormSet:
			return {...state, formId: payload};
		case Pages.ProjectSet:
			return {...state, projectId: payload};
		case Pages.ElementSet:
			return {...state, elementId: payload};
		case Pages.SessionSet:
			return {...state, formId: payload};
	}


	return state;
}

export default pageReducer;