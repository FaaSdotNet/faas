/**
 * Created by Wermington on 27.11.16.
 */
import {
    User
} from '../constants';

import {apiClient} from '../utils';

const COLL_TYPE = User;
const FAIL_TYPE = COLL_TYPE.Fail;
const COLL_NAME = "login";
const URL_ELEM = `/${COLL_NAME}/`;

export class UserActions {

    static logIn(user) {
        return (dispatch) => {
            apiClient.post(URL_ELEM, user)
                .then(res => {
                    console.log(`[POST] ${COLL_NAME}: `, res);
                    dispatch({
                        type: COLL_TYPE.LoginSucc,
                        payload: res.data
                    });
                }).catch((err) => {
                    console.log(`[ERROR] ${COLL_NAME}: `, err);
                    dispatch({ type: FAIL_TYPE, payload: err });
                });
        }
    }

	static loginSuccess(googleToken){
		return {
			type: User.LoginSucc,
			payload: {
				googleToken: googleToken
			}
		}
	}

	static fail(){
		localStorage.removeItem('GoogleToken');
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
		localStorage.removeItem('GoogleToken');
		return {
			type: USER_LOGOUT
		}
	}
}


export default UserActions;