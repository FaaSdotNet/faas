import React from 'react';
import ReactDOM from 'react-dom';
import {Provider} from 'react-redux';
import axios from 'axios';
import {REQUEST} from "../constants";

export function createConstants(...constants)
{
	return constants.reduce((acc, constant) =>
	{
		acc[constant] = constant;
		return acc;
	}, {});
}

export function createReducer(initialState, reducerMap)
{
	return (state = initialState, action) =>
	{
		const reducer = reducerMap[action.type];

		return reducer
			? reducer(state, action.payload)
			: state;
	};
}

export function checkHttpStatus(response)
{
	if (response.status >= 200 && response.status < 300) {
		return response
	} else {
		const error = new Error(response.statusText);
		error.response = response;
		throw error
	}
}

export function parseJSON(response)
{
	return response.json()
}


export function getHeaders(token)
{
	const head = REQUEST.headers;
	return (!token) ? {...head} : {...head, Authorization: `Bearer ${token}`};

}
/*
 * https://github.com/mzabriskie/axios
 */
export const apiClient = axios.create({
	baseURL: REQUEST.base,
	headers: REQUEST.headers
});