/**
 * Created by Wermington on 27.11.16.
 */

import {
	Forms
} from '../constants';

import {apiClient} from "../utils";
const COLL_TYPE = Forms;
const FAIL_TYPE = COLL_TYPE.Fail;
const COLL_NAME = "forms";
const URL_ELEM = `/${COLL_NAME}/`;

export class FormsActions {

	static reset(){
		return (dispatch) => {
			dispatch({type: COLL_TYPE.Reset})
		}
	}

	static fetchAll(projectId, userId) {
		return (dispatch) => {
		    apiClient.get(URL_ELEM + "?projectId="+ projectId + "&userId=" + userId)
				.then((res) => {
					console.log(`[FETCH] ${COLL_NAME}: `, res.data);
					dispatch ({type: COLL_TYPE.FetchSucc, payload: res.data});
				})
				.catch((err) => {
					console.error(`[ERROR] ${COLL_NAME}: `, err);
					dispatch ({type: FAIL_TYPE, payload: err});
				});
		};
	}

	static del(id, userId){
	    return (dispatch) => {
	        console.log("[ACTION] Form delete: ", id);
		    apiClient.delete(URL_ELEM + `${id}?userId=${userId}`)
				.then((res) => {
					console.log(`[DELETE] ${COLL_NAME}: `, res);
					dispatch({type: COLL_TYPE.DeleteSucc, payload: res.data});

				})
				.catch((err) => {
					console.error(`[ERROR] ${COLL_NAME}: `, err);
					dispatch({type: FAIL_TYPE, payload: err});
				});
		};
	}

	static update(form, userId){
		return (dispatch) => {
		    apiClient.put(URL_ELEM + `?userId=${userId}`, form)
				.then(res => {
					console.log(`[UPDATE] ${COLL_NAME}: `, res);
					dispatch({
						type: COLL_TYPE.UpdateSucc,
						payload: res.data
					});
				}).catch((err) =>{
				console.error(`[ERROR] ${COLL_NAME}: `, err);
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}
	static get(id, userId){
		return (dispatch) => {
			apiClient.get(URL_ELEM + id + "?userId=" + userId)
				.then(res => {
					console.log(`[GET] ${COLL_NAME}: `, res);
					dispatch({
						type: COLL_TYPE.GetSucc,
						payload: res.data
					});
				}).catch((err) =>{
				console.log(`[ERROR] ${COLL_NAME}: `, err);
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}

	static create(form)
	{
		return (dispatch) => {
			apiClient.post(URL_ELEM, form)
				.then(res => {
					console.log(`[CREATE] ${COLL_NAME}: `, res);
					dispatch({
						type: COLL_TYPE.CreateSucc,
						payload: res.data
					});
				}).catch((err) =>{
				console.log(`[ERROR] ${COLL_NAME}: `, err);
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}
}

export default FormsActions;



