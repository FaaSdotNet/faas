/**
 * Created by Wermington on 27.11.16.
 */
import {
	Elements
} from '../constants';

import {apiClient} from "../utils";

const COLL_TYPE = Elements;
const FAIL_TYPE = COLL_TYPE.Fail;

const URL_ELEM = "/elements/";

export function fetchAll()
{
	return (dispatch) => {
		apiClient.get(URL_ELEM)
			.then((res) => {
				dispatch ({type: COLL_TYPE.FetchSucc, payload: res});
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
				dispatch({type: COLL_TYPE.DeleteSucc, payload: res});

			})
			.catch((err) => {
				dispatch({type: FAIL_TYPE, payload: err});
			});
	};
}

export function updateElement(element)
{
	return (dispatch) => {
		apiClient.put(URL_ELEM, element)
			.then(res => {
				dispatch({
					type: COLL_TYPE.UpdateSucc,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

export function createElement(element)
{
	return (dispatch) => {
		apiClient.post(URL_ELEM, element)
			.then(res => {
				dispatch({
					type: COLL_TYPE.CreateSucc,
					payload: res
				})
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
					type: COLL_TYPE.GetSucc,
					payload: res
				});
			}).catch((err) =>{
			dispatch({ type: FAIL_TYPE, payload: err });
		});
	}
}

