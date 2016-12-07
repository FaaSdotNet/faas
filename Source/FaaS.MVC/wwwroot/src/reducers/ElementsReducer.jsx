/**
 * Created by Wermington on 27.11.16.
 */


import {
	Elements
} from '../constants';

const initialState = {
	elements: [],
	statusText: null,
	element: null,
	reload: true,
};


export function elementsReducer(state, action){
	const type = action.type;
	const payload = action.payload;
	state= state || initialState;
	switch (type){
		case Elements.Reset:
			return {...initialState};
		case Elements.FetchSucc:
			console.info("[ELEMENTS] Fetch Success:", payload);
			return {...state, elements: payload, reload: false};
		case Elements.Fail:
			console.error("[ELEMENTS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: false};
		case Elements.CreateSucc:
			console.info("[ELEMENTS] create:", payload);
			return {...state, reload: true, element: payload};
		case Elements.GetSucc:
			console.info("[ELEMENTS] get:", payload);
			return {...state, project: payload};
		case Elements.DeleteSucc:
			console.info("[ELEMENTS] delete:", payload);
			return {...state, reload: true};
		case Elements.UpdateSucc:
			console.info("[ELEMENTS] update:", payload);
			return {...state, element: payload, reload:true};
	}
	return {...state};
}


export default elementsReducer;