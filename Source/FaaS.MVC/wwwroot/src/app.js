// src/index.js

import "core-js/fn/object/assign";
import React from "react";
import ReactDOM from "react-dom";
import {Provider} from 'react-redux';
import { browserHistory, hashHistory } from "react-router";
import Root from "./Root";
import store from "./store"

// Render the main component into the dom
const app = document.getElementById('app');
ReactDOM.render(
	<Provider store={store}>
		<Root history={hashHistory} />
	</Provider>
   , app
);