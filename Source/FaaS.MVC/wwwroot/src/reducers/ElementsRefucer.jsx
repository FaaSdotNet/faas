/**
 * Created by Wermington on 27.11.16.
 */
/**
 * Created by Wermington on 27.11.16.
 */

import {
	Elements
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	elements: {},
	statusText: null
};

export default createReducer(initialState, {
	
	[Elements.FetchSucc]: (state, payload) => {
		return {...state, elements: payload};
	},
	[Elements.Fail]: (state, payload) => {
		return {...state, statusText: "[FAIL] Elements: "+ payload};
	},
	[Elements.CreateSucc]: (state, payload) => {
		return {...state, elements: Object.assign({}, state.elements, payload)};
	},
	[Elements.DeleteSucc]: (state, payload) => {
		return state;
	},
	[Elements.GetSucc]: (state, payload) => {
		return {...state}
	},
	[Elements.UpdateSucc]: (state, payload) => {
		return {...state, elements: Object.assign({}, state.elements, payload)};
	}
});

