import React from 'react';
import ReactDOM from "react-dom";
import InputField from './InputField.jsx'

export default class Element extends React.Component {
    constructor(props) {
        super(props);
		
        this.state = {  id: this.props.content.id,
                        description: this.props.content.description,
                        options: JSON.parse(this.props.content.options),
                        type: this.props.content.type,
                     };

    };

    render() {
        return (
            <div>
                <h4>{this.state.description}</h4>
                <InputField type={this.state.type} options={this.state.options} elementId={this.state.id} valueChanged={this.props.valueChanged} />
            </div>
        );
    };
}

//export default  Element;