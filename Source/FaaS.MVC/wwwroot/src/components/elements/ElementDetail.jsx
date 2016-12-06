import React, { Component } from "react";
import {ElementType} from "../../constants";

export class ElementDetail extends Component {

    constructor(props) {
        super(props);
        this.state = {};
    }

    componentWillMount() {
        this.setState(this.props.element);
    }

    render() {
        const state = this.state;
        return (
            <div>
                <h2>Element Details</h2>
                <p>
					<strong>Description:</strong>{state.description}
				</p>
				<p>
                    <strong>Options:</strong>{state.options}
				</p>
				<p>
                    <strong>Type:</strong>{ElementType.to_text[state.type]}
				</p>
				<p>
                    <strong>Required:</strong>{state.required}
				</p>
            </div>
       );
    }
}

export default ElementDetail;