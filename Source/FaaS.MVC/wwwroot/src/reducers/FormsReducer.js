/**
 * Created by Wermington on 27.11.16.
 */
/**
 * Created by Wermington on 27.11.16.
 */

import {
	Forms
} from '../constants';

import {createReducer} from "../utils/index";

const initialState = {
	forms: {},
	statusText: null
};

export default createReducer(initialState, {
	[Forms.FetchSucc]: (state, payload) => {
		return {...state, forms: payload};
	},
	[Forms.Fail]: (state, payload) => {
		return {...state, statusText: "[FAIL] Forms: "+ payload};
	},
	[Forms.CreateSucc]: (state, payload) => {
		return {...state, forms: Object.assign({}, state.forms, payload)};
	},
	[Forms.DeleteSucc]: (state, payload) => {
		return state;
	},
	[Forms.GetSucc]: (state, payload) => {
		return {...state}
	},
	[Forms.UpdateSucc]: (state, payload) => {
		return {...state, forms: Object.assign({}, state.forms, payload)};
	}
});

