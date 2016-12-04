import {Pages} from '../constants';

export class PagesActions {
	static setPage(id) {
		return (dispatch) => {
			dispatch({type: Pages.PageSet, payload: id})
		}
	}

	static setForm(id) {
		return (dispatch) => {
			dispatch({type: Pages.FormSet, payload: id})
		}
	}

	static setProject(id) {
		return (dispatch) => {
			dispatch({type: Pages.ProjectSet, payload: id})
		}
	}

}

export default PagesActions;


