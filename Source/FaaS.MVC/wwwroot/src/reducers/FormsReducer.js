/**
 * Created by Wermington on 27.11.16.
 */
/**
 * Created by Wermington on 27.11.16.
 */

import {
	FORMS_SET,
	FORMS_UNSET,
	FORMS_CREATE_SUCC,
	FORMS_FETCH_SUCC,
	FORMS_ANY_FAIL,
	FORMS_DELETE_SUCC,
	FORMS_UPDATE_SUCC,
	FORMS_GET_SUCC
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	currentForm: null,
	forms: {},
	statusText: null
};

export default createReducer(initialState, {
	[FORMS_SET]: (state, payload) =>
	{
		return {...state, currentForm: payload};
	},
	[FORMS_UNSET]: (state, payload) => {
		return {...state, currentForm: null};

	},
	[FORMS_FETCH_SUCC]: (state, payload) => {
		return {...state, forms: payload};
	},
	[FORMS_ANY_FAIL]: (state, payload) => {
		return {...state, statusText: "[FAIL]: "+ payload};
	},
	[FORMS_CREATE_SUCC]: (state, payload) => {
		return {...state, forms: Object.assign({}, state.forms, payload), currentForm: payload.Id};
	},
	[FORMS_DELETE_SUCC]: (state, payload) => {
		return state;
	},
	[FORMS_GET_SUCC]: (state, payload) => {
		return {...state, currentForm: payload.Id}
	},
	[FORMS_UPDATE_SUCC]: (state, payload) => {
		return {...state, forms: Object.assign({}, state.forms, payload), currentForm: payload.Id};
	}
});

