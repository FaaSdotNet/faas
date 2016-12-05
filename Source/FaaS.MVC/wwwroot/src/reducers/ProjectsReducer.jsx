/**
 * Created by Wermington on 27.11.16.
 */

import {
	Projects
} from '../constants';


const initialState = {
	projects: [],
	statusText: null,
	project: null,
	reload: true
};

export function projectsReducer(state, action){
	state = state || initialState;
	const payload = action.payload;
	const type = action.type;

	switch (type){
		case Projects.Reset:
			return {...initialState};
		case Projects.FetchSucc:
			console.info("[PROJECTS] setting projects:", payload);
			return {...state, projects: payload, reload: false};
		case Projects.Fail:
			console.error("[PROJECTS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload, reload: true};
		case Projects.CreateSucc:
			console.info("[PROJECTS] create:", payload);
			return {...state, reload: true, project: payload};
		case Projects.GetSucc:
			console.info("[PROJECTS] get:", payload);
			return {...state, project: payload};
		case Projects.DeleteSucc:
			console.info("[PROJECTS] delete:", payload);
			return {...state, reload: true};
		case Projects.UpdateSucc:
			console.info("[PROJECTS] update:", payload);
			return {...state, project: payload, reload:true};
	}
	return {...state};
}

export default projectsReducer;