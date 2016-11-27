/**
 * Created by Wermington on 27.11.16.
 */

import { pushState } from 'redux-router';
import {USER_LOGIN_SUCC, USER_LOGIN_FAIL, USER_LOGIN, USER_LOGOUT} from '../constants';
import {apiClient} from "../utils"

export function loginUserSuccess(token) {
	localStorage.setItem('token', token);
	return {
		type: USER_LOGIN_SUCC,
		payload: {
			token: token
		}
	}
}


export function loginUserFailure(error) {
	localStorage.removeItem('token');
	return {
		type: USER_LOGIN_FAIL,
		payload: {
			status: error.response.status,
			statusText: error.response.statusText
		}
	}
}


export function loginUserRequest() {
	return {
		type: USER_LOGIN
	}
}

export function fetchUser(id) {
	return (dispatch) => {
		apiClient.get("/users/" + id)
			.then((res) => {
				r
			});
	};
}

export function logout() {
	localStorage.removeItem('token');
	return {
		type: USER_LOGOUT
	}
}