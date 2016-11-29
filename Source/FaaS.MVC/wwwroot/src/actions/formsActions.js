/**
 * Created by Wermington on 27.11.16.
 */

import {
	Forms
} from '../constants';

import {apiClient} from "../utils";
const COLL_TYPE = Forms;
const FAIL_TYPE = COLL_TYPE.Fail;
const URL_ELEM = "/forms/";

export class FormsActions {
	static fetchAll() {
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

	static del(id){
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

	static updateForm(element){
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
	static get(id){
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

	static create(element)
	{
		return (dispatch) => {
			apiClient.post(URL_ELEM, element)
				.then(res => {
					dispatch({
						type: COLL_TYPE.CreateSucc,
						payload: res
					});
				}).catch((err) =>{
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}
}

export default FormsActions;



