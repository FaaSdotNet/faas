/**
 * Created by Wermington on 27.11.16.
 */

import {
	Projects
} from '../constants';


const initialState = {
	projects: [],
	statusText: null
};

export function projectsReducer(state, action){
	state = state || initialState;
	console.log("[REDUCER] Projects: ", action);

	const payload = action.payload;
	const type = action.type;

	switch (type){
		case Projects.FetchSucc:
			console.info("[PROJECTS] setting projects:", payload);
			return {...state, projects: payload};
		case Projects.Fail:
			console.error("[PROJECTS] FAIL:", payload);
			return {...state, statusText: "[FAIL]: "+ payload};
		case Projects.CreateSucc:
			console.info("[PROJECTS] create:", payload);
			return {...state, projects: Object.assign({}, state.projects, payload)};
		case Projects.GetSucc:
			console.info("[PROJECTS] get:", payload);
			return {...state, currentProject: payload.Id};
		case Projects.DeleteSucc:
			console.info("[PROJECTS] delete:", payload);
			return {...state};
		case Projects.UpdateSucc:
			console.info("[PROJECTS] update:", payload);
			return {...state, projects: Object.assign({}, state.projects, payload)};
	}

	return state;
}

export default projectsReducer;