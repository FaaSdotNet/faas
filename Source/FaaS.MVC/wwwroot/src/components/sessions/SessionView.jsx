import React, { Component } from "react";
import Element from '../form/Element.jsx';

export class SessionView extends Component {

    constructor(props) {
        super(props);

        this.state = {
            form: {},
            elements: [],
            elementValues: {},
            values: {},
            session: {}
        };
    };

    componentWillMount() {
        const result = fetch(`/api/v1.0/sessions/${this.props.params.sessionId}`,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json'
            }
        });

        result.then((res) => {
            if (res.ok) {
                res.json()
                    .then((js) => {
                        let elements = [];
                        js.elementValues.forEach((e) => elements.push(e.element));
                        this.setState({
                            elements: elements,
                            form: js.elementValues[0].element.form,
                            elementValues: js.elementValues,
                            session: js
                        });
                    });
            }
        });
    }

    render() {
        let elementsList = [];
        let value = null;
        for (let key in this.state.elements) {
            if (this.state.elements.hasOwnProperty(key)) {
                this.state.elementValues.forEach(
                    (e) => {
                        const formElementId = this.state.elements[key].id;
                        if (e.element.id == formElementId) {
                            value = e.value;
                        }
                    });
                elementsList.push(
                    <Element ref={"elem" + key} key={"element" + key} content={this.state.elements[key]} valueChanged={
this.props.valueChanged} defaultValue={value}/>
                );
            }
        }

        return (
            <div>
                <h1>
                    <strong>Session:</strong>
                </h1>
                <pre>
					<p><emph>(Filled at: {this.state.session.filled})</emph></p>
					<p><emph>(For form: {this.state.form.formName})</emph></p>
                    </pre>
                <div id="elemContainer">
                    <div className="form-group">
                        {elementsList}

                    </div>
                </div>
            </div>
        );
    }
}

export default SessionView;