import React from 'react';
import {browserHistory } from 'react-router';
import Element from './form/Element.jsx'
import MySubmit from "./form/MySubmit.jsx"

export default class Form extends React.Component {
    constructor(props) {
        super(props);
		this.state = { 
            elements: this.props.model.elements,
            values: {}
        };

        console.log(this.props)
    };

    handleSubmit(e) {
        const valuesToPost = [];
        let i = 0;

        for(const key in this.state.elements){

            if(this.state.elements.hasOwnProperty(key)) {
                valuesToPost[i] = this.state.values[this.state.elements[key].id];
                i++;
            }
        }

        //var stringToSend = '{jsonString: "' +  JSON.stringify(this.state.values) + '" }'
        const postObj = {};
        postObj.form = this.props.model.form;
        postObj.elements = this.state.elements;
        postObj.values = valuesToPost;
        const stringToSend = JSON.stringify({
            Form: this.props.model.form,
            Elements: this.state.elements,
            Values: valuesToPost
        });

        console.log(stringToSend);

        const result = fetch('/api/v1.0/form', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: "same-origin",
            body: stringToSend
        });

        result.then( (res) =>  {
            if (res.ok) {
                browserHistory.push("/form/thankyou");
            }
        });
    }

    valueChanged(elementId, newVal){
        const values = this.state.values;
        values[elementId] = newVal;
        this.setState({values: values}); 
    }

    render() {
        const elementsList = [];
        for(const key in this.state.elements){
            if(this.state.elements.hasOwnProperty(key)){
                elementsList.push(<li key={key}><Element content={this.state.elements[key]} valueChanged={this.valueChanged.bind(this)}/></li>)
            }
        }

        return (
            <div id="elemContainer">
                <h1>{this.props.model.form.formName}</h1>
                {elementsList}
                <MySubmit onClick={this.handleSubmit.bind(this)} id="formButton" value="Submit"/>
            </div>
        );
    };
}

//export default  Form;