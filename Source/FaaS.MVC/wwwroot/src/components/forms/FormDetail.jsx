import React, { Component } from "react";

export class FormDetail extends Component {

    constructor(props) {
        super(props);

        this.state = {};
    }

    componentWillMount() {
        const result = fetch('/api/v1.0/forms/' + this.props.formId,
        {
            method: "GET",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json'
            }
        });

        result.then( (res) =>  {
            if(res.ok) {
                res.json()
                    .then((js) => {
                        this.setState(js);
                    });
            }
        });
    }

    render() {
        const state = this.state;
        return (
            <div>
				<h1>
					<strong>Form:</strong> {state.formName}
				</h1>
				<pre>
                        {state.description}
					<p><emph>(Created: {state.created})</emph></p>
                    </pre>

			</div>
       );
    }
}

export default FormDetail;