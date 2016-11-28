/**
 * Created by Wermington on 27.11.16.
 */
import {
	ELEMENTS_FETCH_SUCC,
	ELEMENTS_ANY_FAIL,
	ELEMENTS_DELETE_SUCC,
	ELEMENTS_UPDATE_SUCC,
	ELEMENTS_CREATE_SUCC,
	ELEMENTS_GET_SUCC
} from '../constants';

import {apiClient} from "../utils";

const FAIL_TYPE = ELEMENTS_ANY_FAIL;
const URL_ELEM = "/elements/";

export function fetchAll()
{
	return (dispatch) => {
		apiClient.get(URL_ELEM)
			.then((res) => {
				dispatch ({type: ELEMENTS_FETCH_SUCC, payload: res});
			})
			.catch((err) => {
				dispatch ({type: FAIL_TYPE, payload: err});
			});
	};
}

export function deleteElement(id)
{
	return (dispatch) => {
		apiClient.delete(URL_ELEM + id)
			.then((res) => {
				dispatch({type: ELEMENTS_DELETE_SUCC, payload: res});

			})
			.catch((err) => {
				dispatch({type: FAIL_TYPE, payload: err});
			});
	};
}

export function updateElement(project)
{
	return (dispatch) => {
		apiClient.put(URL_ELEM, project)
			.then(res => {
				dispatch({
					type: ELEMENTS_UPDATE_SUCC,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function getElement(id)
{
	return (dispatch) => {
		apiClient.get(URL_ELEM + id)
			.then(res => {
				dispatch({
					type: ELEMENTS_GET_SUCC,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

