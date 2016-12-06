/**
 * Created by Wermington on 27.11.16.
 */


import {
	Forms
} from '../constants';

const initialState = {
	forms: [],
	statusText: null,
	form: null,
	reload: true
};


export function formsReducer(state, action){
	const type = action.type;
	const payload = action.payload;
	state= state || initialState;
	switch (type){
		case Forms.Reset:
			return {...initialState};
		case Forms.FetchSucc:
			console.info("[FORMS] Fetch Success:", payload);
			return {...state, forms: payload, reload: false};
		case Forms.Fail:
			console.error("[FORMS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: true};
		case Forms.CreateSucc:
			console.info("[FORMS] create:", payload);
			return {...state, reload: true, form: payload};
		case Forms.GetSucc:
			console.info("[FORMS] get:", payload);
			return {...state, project: payload};
		case Forms.DeleteSucc:
			console.info("[FORMS] delete:", payload);
			return {...state, reload: true};
		case Forms.UpdateSucc:
			console.info("[FORMS] update:", payload);
			return {...state, form: payload, reload:true};
	}
	return {...state};
}


export default formsReducer;