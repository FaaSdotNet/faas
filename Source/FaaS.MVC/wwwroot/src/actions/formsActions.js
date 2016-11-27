/**
 * Created by Wermington on 27.11.16.
 */

import {
	FORMS_SET,
	FORMS_UNSET,
	FORMS_FETCH,
	FORMS_FETCH_SUCC,
	FORMS_ANY_FAIL,
	FORMS_DELETE_SUCC,
	FORMS_UPDATE_SUCC,
	FORMS_GET_SUCC,
	FORM_CREATE_SUCC
} from '../constants';

import {apiClient} from "../utils";

const FAIL_TYPE = FORMS_ANY_FAIL;
const URL_ELEM = "/forms/";

export function fetchAll()
{
	return (dispatch) => {
		apiClient.get(URL_ELEM)
			.then((res) => {
				dispatch ({type: FORMS_FETCH_SUCC, payload: res});
			})
			.catch((err) => {
				dispatch ({type: FAIL_TYPE, payload: err});
			});
	};
}

export function deleteForm(id)
{
	return (dispatch) => {
		apiClient.delete(URL_ELEM + id)
			.then((res) => {
				dispatch({type: FORMS_DELETE_SUCC, payload: res});

			})
			.catch((err) => {
				dispatch({type: FAIL_TYPE, payload: err});
			});
	};
}


export function createForm(form)
{
	return (dispatch) => {
		apiClient.post(URL_ELEM, form)
			.then((res) => {
				dispatch({type: FORM_CREATE_SUCC, payload: res});

			})
			.catch((err) => {
				dispatch({type: FAIL_TYPE, payload: err});
			});
	};
}

export function updateForm(form)
{
	return (dispatch) => {
		apiClient.put(URL_ELEM, form)
			.then(res => {
				dispatch({
					type: FORMS_UPDATE_SUCC,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function getForm(id)
{
	return (dispatch) => {
		apiClient.get(URL_ELEM + id)
			.then(res => {
				dispatch({
					type: FORMS_GET_SUCC,
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
		type: FORMS_SET,
		payload: id
	}
}

export function unsetCurrent(id)
{
	return {
		type: FORMS_UNSET,
		payload: null
	}
}
