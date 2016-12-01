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
	elements: [],
	statusText: null
};

export function elementsReducer(state, action){
	const type = action.type;
	const payload = action.payload;
	state= state || initialState;
	console.log("[REDUCER] Elements: ", action);
	switch (type)
	{
		case Elements.FetchSucc:
			return {...state, elements: payload};
		case Elements.Fail:
			return {...state, statusText: "[FAIL] Elements: "+ payload};
		case Elements.CreateSucc:
			return {...state, elements: Object.assign({}, state.elements, payload)};
		case Elements.DeleteSucc:
			return {...state};
		case Elements.GetSucc:
			return {...state};
		case Elements.UpdateSucc:
			return {...state, elements: Object.assign({}, state.elements, payload)};
	}
	return state;
}


export default elementsReducer;

