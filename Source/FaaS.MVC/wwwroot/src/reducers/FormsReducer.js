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


export function formsReducer(state, action){
	const type = action.type;
	const payload = action.payload;
	state= state || initialState;
	switch (type)
	{
		case Forms.FetchSucc:
			return {...state, forms: payload};
		case Forms.Fail:
			return {...state, statusText: "[FAIL] Elements: "+ payload};
		case Forms.CreateSucc:
			return {...state, forms: Object.assign({}, state.forms, payload)};
		case Forms.DeleteSucc:
			return {...state};
		case Forms.GetSucc:
			return {...state};
		case Forms.UpdateSucc:
			return {...state, forms: Object.assign({}, state.forms, payload)};
	}
	return state;
}


export default formsReducer;