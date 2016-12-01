/**
 * Created by Wermington on 27.11.16.
 */
import {
	Projects
} from '../constants';

import {apiClient} from "../utils";

const COLL_TYPE = Projects;
const FAIL_TYPE = COLL_TYPE.Fail;
const COLL_NAME = "projects";
const URL_ELEM = `/${COLL_NAME}/`;

export class ProjectsActions {
	static fetchAll(userId) {
		return (dispatch) => {
			apiClient.get(URL_ELEM)
				.then((res) => {
					console.log(`[FETCH] ${COLL_NAME}: `, res);
					dispatch ({type: COLL_TYPE.FetchSucc, payload: res});
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
					dispatch({type: COLL_TYPE.DeleteSucc, payload: res});

				})
				.catch((err) => {
					console.error(`[ERROR] ${COLL_NAME}: `, err);
					dispatch({type: FAIL_TYPE, payload: err});
				});
		};
	}

	static update(project){
		return (dispatch) => {
			apiClient.put(URL_ELEM, project)
				.then(res => {
					console.log(`[UPDATE] ${COLL_NAME}: `, res);
					dispatch({
						type: COLL_TYPE.UpdateSucc,
						payload: res
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
						payload: res
					});
				}).catch((err) =>{
				console.log(`[ERROR] ${COLL_NAME}: `, err);
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}

	static create(userId, project)
	{
		return (dispatch) => {
			apiClient.post(URL_ELEM, project)
				.then(res => {
					console.log(`[CREATE] ${COLL_NAME}: `, res);
					dispatch({
						type: COLL_TYPE.CreateSucc,
						payload: res
					});
				}).catch((err) =>{
				console.log(`[ERROR] ${COLL_NAME}: `, err);
				dispatch({ type: FAIL_TYPE, payload: err });
			});
		}
	}
}

export default ProjectsActions;
