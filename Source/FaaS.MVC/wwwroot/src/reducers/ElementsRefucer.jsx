/**
 * Created by Wermington on 27.11.16.
 */
/**
 * Created by Wermington on 27.11.16.
 */

import {
	ELEMENTS_CREATE_SUCC,
	ELEMENTS_FETCH_SUCC,
	ELEMENTS_ANY_FAIL,
	ELEMENTS_DELETE_SUCC,
	ELEMENTS_UPDATE_SUCC,
	ELEMENTS_GET_SUCC
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	elements: {},
	statusText: null
};

export default createReducer(initialState, {
	
	[ELEMENTS_FETCH_SUCC]: (state, payload) => {
		return {...state, elements: payload};
	},
	[ELEMENTS_ANY_FAIL]: (state, payload) => {
		return {...state, statusText: "[FAIL]: "+ payload};
	},
	[ELEMENTS_CREATE_SUCC]: (state, payload) => {
		return {...state, elements: Object.assign({}, state.elements, payload)};
	},
	[ELEMENTS_DELETE_SUCC]: (state, payload) => {
		return state;
	},
	[ELEMENTS_GET_SUCC]: (state, payload) => {
		return {...state}
	},
	[ELEMENTS_UPDATE_SUCC]: (state, payload) => {
		return {...state, elements: Object.assign({}, state.elements, payload)};
	}
});

