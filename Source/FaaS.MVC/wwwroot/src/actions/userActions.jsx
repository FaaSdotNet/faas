/**
 * Created by Wermington on 27.11.16.
 */

import { pushState } from 'redux-router';
import {User} from '../constants';
import {apiClient} from "../utils"


export class UserActions {
	static loginSuccess(token){
		localStorage.setItem('token', token);
		return {
			type: User.LoginSucc,
			payload: {
				token: token
			}
		}
	}

	static fail(){
		localStorage.removeItem('token');
		return {
			type: User.Fail,
			payload: {
				status: error.response.status,
				statusText: error.response.statusText
			}
		}
	}

	static loginRequest(){
		return { type: User.LoginReq, payload: 0 }
	}

	static fetch() {
		return (dispatch) => {
			apiClient.get('/users/')
				.then((res) => {
					dispatch ({type: User.FetchSucc, payload: res});
				})
				.catch((err) => {
					dispatch ({type: User.Fail, payload: err});
				});
		};
	}

	static logout() {
		localStorage.removeItem('token');
		return {
			type: USER_LOGOUT
		}
	}
}


export default UserActions;