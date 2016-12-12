/**
 * Created by Wermington on 27.11.16.
 */
import {
	Sessions
} from '../constants';

import {apiClient} from "../utils";

const COLL_TYPE = Sessions;
const FAIL_TYPE = COLL_TYPE.Fail;
const COLL_NAME = "sessions";
const URL_ELEM = `/${COLL_NAME}/`;

export class SessionsActions {

	static reset(){
		return (dispatch) => {
			dispatch({type: COLL_TYPE.Reset})
		}
	}

	static fetchAll(formId) {
		return (dispatch) => {
			apiClient.get(URL_ELEM + "?formId="+ formId)
				.then((res) => {
					console.log(`[FETCH] ${COLL_NAME}: `, res);
					dispatch({type: COLL_TYPE.FetchSucc, payload: res.data});
				})
				.catch((err) => {
					console.error(`[ERROR] ${COLL_NAME}: `, err);
					dispatch({type: FAIL_TYPE, payload: err});
				});
		};
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
}

export default SessionsActions;
