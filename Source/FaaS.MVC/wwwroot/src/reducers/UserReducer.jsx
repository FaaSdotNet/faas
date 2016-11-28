import {USER_LOGIN_REQUEST,USER_LOGIN_SUCC, USER_LOGIN_FAIL, USER_LOGOUT} from '../constants';
import {createReducer} from "../utils/index";

const initialState = {
	token: null,
	userName: null,
	isAuthenticated: false,
	isAuthenticating: false,
	statusText: null
};

export default createReducer(initialState, {
	[USER_LOGIN_REQUEST]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': true,
			'statusText': null
		});
	},
	[USER_LOGIN_SUCC]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': false,
			'isAuthenticated': true,
			'token': payload.token,
			'userName': jwtDecode(payload.token).userName,
			'statusText': 'You have been successfully logged in.'
		});

	},
	[USER_LOGIN_FAIL]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': false,
			'isAuthenticated': false,
			'token': null,
			'userName': null,
			'statusText': `Authentication Error: ${payload.status} ${payload.statusText}`
		});
	},
	[USER_LOGOUT]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticated': false,
			'token': null,
			'userName': null,
			'statusText': 'You have been successfully logged out.'
		});
	}
});


