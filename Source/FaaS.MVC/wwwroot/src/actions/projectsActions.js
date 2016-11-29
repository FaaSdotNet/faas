/**
 * Created by Wermington on 27.11.16.
 */
import {
	Projects
} from '../constants';

import {apiClient} from "../utils";

const COLL_TYPE = Projects;
const FAIL_TYPE = COLL_TYPE.Fail;
const URL_ELEM = "/projects/";

export function fetchAll()
{
	return (dispatch) => {
		apiClient.get(URL_ELEM)
			.then((res) => {
				dispatch ({type: PROJECTS_FETCH_SUCC, payload: res});
			})
			.catch((err) => {
				dispatch ({type: FAIL_TYPE, payload: err});
			});
	};
}

export function deleteProject(id)
{
	return (dispatch) => {
		apiClient.delete(URL_ELEM + id)
			.then((res) => {
				dispatch({type: PROJECTS_DELETE_SUCC, payload: res});

			})
			.catch((err) => {
				dispatch({type: FAIL_TYPE, payload: err});
			});
	};
}

export function createProject(project)
{
	return (dispatch) => {
		apiClient.post(URL_ELEM, project)
			.then(res => {
				dispatch({
					type: PROJECTS_CREATE_SUCC,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function updateProject(project)
{
	return (dispatch) => {
		apiClient.put(URL_ELEM, project)
			.then(res => {
			dispatch({
				type: PROJECTS_UPDATE_SUCC,
				payload: res
			});
		}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function getProject(id)
{
	return (dispatch) => {
		apiClient.get(URL_ELEM + id)
			.then(res => {
				dispatch({
					type: PROJECTS_GET_SUCC,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function setCurrent(id)
{
	return {
		type: PROJECTS_SET,
		payload: id
	}
}

export function unsetCurrent(id)
{
	return {
		type: PROJECTS_UNSET,
		payload: id
	}
}
