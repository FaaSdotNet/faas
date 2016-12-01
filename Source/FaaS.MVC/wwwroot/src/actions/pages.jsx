import {Pages} from '../constants';

export class PagesActions {
	static setPage(name) {
		return (dispatch) => {
			dispatch({type: Pages.PageSet, payload: name})
		}
	}

	static setForm(name) {
		return (dispatch) => {
			dispatch({type: Pages.FormSet, payload: name})
		}
	}

	static setProject(name) {
		return (dispatch) => {
			dispatch({type: Pages.ProjectSet, payload: name})
		}
	}
}

export default PagesActions;


