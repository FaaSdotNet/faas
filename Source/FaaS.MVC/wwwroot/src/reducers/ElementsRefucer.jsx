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
			console.info("[FORMS] Fetch Success:", payload);
			return {...state, elements: payload, reload: false};
		case Elements.Fail:
			console.error("[FORMS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: false};
		case Elements.CreateSucc:
			console.info("[FORMS] create:", payload);
			return {...state, reload: true, element: payload};
		case Elements.GetSucc:
			console.info("[FORMS] get:", payload);
			return {...state, project: payload};
		case Elements.DeleteSucc:
			console.info("[FORMS] delete:", payload);
			return {...state, reload: true};
		case Elements.UpdateSucc:
			console.info("[FORMS] update:", payload);
			return {...state, element: payload, reload:true};
	}
	return {...state};
}


export default elementsReducer;