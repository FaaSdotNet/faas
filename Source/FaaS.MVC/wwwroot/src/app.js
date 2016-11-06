// src/index.js

import "core-js/fn/object/assign";
import React from "react";
import ReactDOM from "react-dom";
import { browserHistory, hashHistory } from "react-router";
import Root from "./Root";

// Render the main component into the dom
ReactDOM.render(<Root history={hashHistory} />, document.getElementById('app'));