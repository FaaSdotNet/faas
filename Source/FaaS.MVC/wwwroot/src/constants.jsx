
export const REQUEST = {
	headers: {
		'Content-Type': "application/json",
		'Accept': 'application/json'
	},
	base: '/api/v1.0/'
};

export const PagesLoc = {
	Projects: "projects",
	Forms: "forms",
	Project: "project",
	Form: "form",
	Elements: "elements",
	Element: "element",
};

export const Users = {
	Fetch: 'USERS_FETCH',
	FetchSucc: 'USERS_FETCH_SUCC',
	Fail: 'USERS_ANY_FAIL',
	DeleteSucc: 'USERS_DELETE_SUCC',
	UpdateSucc: 'USERS_UPDATE_SUCC',
	CreateSucc: 'USERS_CREATE_SUCC',
	GetSucc: 'USERS_GET_SUCC',
	Reset: 'USERS_RESET'
};

export const Projects = {
	Fetch: 'PROJECTS_FETCH',
	FetchSucc: 'PROJECTS_FETCH_SUCC',
	Fail: 'PROJECTS_ANY_FAIL',
	DeleteSucc: 'PROJECTS_DELETE_SUCC',
	UpdateSucc: 'PROJECTS_UPDATE_SUCC',
	CreateSucc: 'PROJECTS_CREATE_SUCC',
	GetSucc: 'PROJECTS_GET_SUCC',
	Reset: 'PROJECTS_RESET'
};

export const Forms = {
	Fetch: 'FORMS_FETCH',
	FetchSucc: 'FORMS_FETCH_SUCC',
	Fail: 'FORMS_ANY_FAIL',
	DeleteSucc: 'FORMS_DELETE_SUCC',
	UpdateSucc: 'FORMS_UPDATE_SUCC',
	CreateSucc: 'FORMS_CREATE_SUCC',
	GetSucc: 'FORMS_GET_SUCC',
	Reset: 'FORMS_RESET'

};

export const User = {
	LoginReq: 'USER_LOGIN_REQUEST',
	Fail: 'USER_ANY_FAIL',
	LoginSucc: 'USER_LOGIN_SUCC',
	Get: 'USER_GET_SUCC',
	UpdateSucc: 'USER_UPDATE_SUCC',
	DeleteSucc: 'USER_DELETE_SUCC',
	RegisterSucc: 'USER_REGISTER_SUCC',
	Logout: 'USER_LOGOUT',
};

export const Pages = {
	PageSet: 'PAGE_SET',
	FormSet: 'FORM_SET',
	ProjectSet: 'PROJECT_SET',
	ElementSet: 'ELEMENT_SET'
};

export const Elements = {
	Fetch: 'ELEMENTS_FETCH',
	FetchSucc: 'ELEMENTS_FETCH_SUCC',
	Fail: 'ELEMENTS_ANY_FAIL',
	DeleteSucc: 'ELEMENTS_DELETE_SUCC',
	UpdateSucc: 'ELEMENTS_UPDATE_SUCC',
	CreateSucc: 'ELEMENTS_CREATE_SUCC',
	GetSucc: 'ELEMENTS_GET_SUCC',
	Reset: 'ELEMENTS_RESET'
};

export const ElementType = {
	to_text: [
		'CheckBox',
		'Date',
		'Radio',
		'Range',
		'Text',
        'TextArea'
	],
	CheckBox: 0,
	Date: 1,
	Radio: 2,
	Range: 3,
	Text: 4,
    TextArea: 5
};
