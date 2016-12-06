/**
 * Created by Wermington on 27.11.16.
 */

import {
	Users
} from '../constants';


const initialState = {
	users: [],
	statusText: null,
	user: null,
	reload: true
};

export function UsersReducer(state, action){
	state = state || initialState;
	const payload = action.payload;
	const type = action.type;

	switch (type){
		case Users.Reset:
			return {...initialState};
		case Users.FetchSucc:
			console.info("[PROJECTS] setting users:", payload);
			return {...state, users: payload, reload: false};
		case Users.Fail:
			console.error("[PROJECTS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: true};
		case Users.CreateSucc:
			console.info("[PROJECTS] create:", payload);
			return {...state, reload: true, user: payload};
		case Users.GetSucc:
			console.info("[PROJECTS] get:", payload);
			return {...state, user: payload};
		case Users.DeleteSucc:
			console.info("[PROJECTS] delete:", payload);
			return {...state, reload: true};
		case Users.UpdateSucc:
			console.info("[PROJECTS] update:", payload);
			return {...state, user: payload, reload:true};
	}
	return {...state};
}

export default UsersReducer;