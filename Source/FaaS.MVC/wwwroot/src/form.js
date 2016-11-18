import React from 'react';
import ReactDOM from "react-dom";
import { Router, Route} from 'react-router'
import Form from './components/Form.jsx';

const queryString = require('query-string');
const parsed = queryString.parse(location.search);
const formId = parsed.formId;
if(formId == null){
    document.location.href="/";
};


const model = window.formElements || [];

ReactDOM.render(<Form model={model}/>, document.getElementById('react-root'));
