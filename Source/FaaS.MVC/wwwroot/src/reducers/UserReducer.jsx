import {User} from '../constants';
import {createReducer} from "../utils/index";

const initialState = {
	token: null,
	userName: null,
	isAuthenticated: false,
	isAuthenticating: false,
	statusText: null
};

export default createReducer(initialState, {
	[User.LoginReq]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': true,
			'statusText': null
		});
	},
	[User.LoginSucc]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': false,
			'isAuthenticated': true,
			'token': payload.token,
			'userName': jwtDecode(payload.token).userName,
			'statusText': 'You have been successfully logged in.'
		});

	},
	[User.Fail]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticating': false,
			'isAuthenticated': false,
			'token': null,
			'userName': null,
			'statusText': `Authentication Error: ${payload.status} ${payload.statusText}`
		});
	},
	[User.Logout]: (state, payload) =>
	{
		return Object.assign({}, state, {
			'isAuthenticated': false,
			'token': null,
			'userName': null,
			'statusText': 'You have been successfully logged out.'
		});
	}
});


