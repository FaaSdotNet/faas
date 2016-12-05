import {User} from '../constants';
import {createReducer} from "../utils/index";

const initialState = {
	token: null,
	userName: null,
	userId: null,
	isAuthenticated: false,
	isAuthenticating: false,
	statusText: null,
	reload: true
};

export function userReducer(state, action)
{
	state = state || initialState;
	const payload = action.payload;
	const type = action.type;
	switch (type){
		case User.LoginReq:
			return Object.assign({}, state, {
				isAuthenticating: true,
				statusText: null
			});
		case User.LoginSucc:
			return Object.assign({}, state, {
				'isAuthenticating': false,
				'isAuthenticated': true,
				'token': payload.token,
				'userName': payload.userName,
				'statusText': 'You have been successfully logged in.'
			});
		case User.Fail:
			return Object.assign({}, state, {
				'isAuthenticating': false,
				'isAuthenticated': false,
				'token': null,
				'userName': null,
				'statusText': `Authentication Error: ${payload.status} ${payload.statusText}`
			});
		case User.Logout:
			return Object.assign({}, state, {
				'isAuthenticated': false,
				'token': null,
				'userName': null,
				'statusText': 'You have been successfully logged out.'
			});

	}

	return state;
}


export default userReducer;


