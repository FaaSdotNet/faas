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

	static fetchAll(projectId) {
		return (dispatch) => {
			apiClient.get(URL_ELEM + "?projectId="+ projectId)
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

	static del(id){
		return (dispatch) => {
			apiClient.delete(URL_ELEM + id)
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

	static update(element){
		return (dispatch) => {
			apiClient.put(URL_ELEM, element)
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
	static get(id){
		return (dispatch) => {
			apiClient.get(URL_ELEM + id)
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

	static create(projectId, element)
	{
		return (dispatch) => {
			apiClient.post(URL_ELEM, element)
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



