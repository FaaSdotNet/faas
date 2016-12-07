/**
 * Created by Wermington on 27.11.16.
 */

import {
	Sessions
} from '../constants';


const initialState = {
	sessions: [],
	statusText: null,
	session: null,
	reload: true
};

export function sessionsReducer(state, action){
	state = state || initialState;
	const payload = action.payload;
	const type = action.type;

	switch (type){
		case Sessions.Reset:
			return {...initialState};
		case Sessions.FetchSucc:
			console.info("[SESSIONS] setting sessions:", payload);
			return {...state, sessions: payload, reload: false};
		case Sessions.Fail:
			console.error("[SESSIONS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: true};
		case Sessions.CreateSucc:
			console.info("[SESSIONS] create:", payload);
			return {...state, reload: true, session: payload};
		case Sessions.GetSucc:
			console.info("[SESSIONS] get:", payload);
			return {...state, session: payload};
		case Sessions.DeleteSucc:
			console.info("[SESSIONS] delete:", payload);
			return {...state, reload: true};
		case Sessions.UpdateSucc:
			console.info("[SESSIONS] update:", payload);
			return {...state, session: payload, reload:true};
	}
	return {...state};
}

export default sessionsReducer;