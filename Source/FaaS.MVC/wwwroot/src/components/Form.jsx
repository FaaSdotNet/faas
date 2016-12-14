import React from 'react';
import Element from './form/Element.jsx'
import MySubmit from "./form/MySubmit.jsx"
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';
import {connect} from "react-redux";

@connect((store) => {
   return store;
})
class Form extends React.Component {
    constructor(props) {
        super(props);

        this.openModal = this.openModal.bind(this);
        this.redirectToHome = this.redirectToHome.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.valueChanged = this.valueChanged.bind(this);

		this.state = { 
            isOpen: false,
            form: {},
            elements: [],
            values: {}
        };
    };

    openModal() {
        this.setState({
            isOpen: true
        });
    };

    redirectToHome(){
        this.setState({
            isOpen: false
        });
        document.location.href = "/";
    }

    handleReturn()
	{
		document.location.href="/#/forms/";
	}

	componentWillMount()
	{
		const result = fetch('/api/v1.0/form?formId=' + this.props.params.formid,
			{
				method: "GET",
				credentials: "same-origin",
				headers: {
					'Content-Type': "application/json"
				}
			});

		result.then((res) =>
		{
			if (res.ok) {
				res.json()
					.then((js) =>
					{
						this.setState({elements: js.elements, form: js.form, values: {}});
					});
			}
		});
	}

    handleSubmit() {
        let valuesToPost = [];
        let i = 0;
        for(let key in this.state.elements){
            valuesToPost[i] = this.state.values[this.state.elements[key].id];
            i++;
        }

        let incomplete = false;
        for(let key in this.state.elements){
            if(this.state.elements[key].required && (valuesToPost[key] == null || valuesToPost[key] == "")){
                incomplete = true;
                this.refs["elem"+key].setRequiredLabel(true);
            } else {
                this.refs["elem"+key].setRequiredLabel(false);
            }

        }

        if(incomplete){
            return;
        }
        
        let postObj = {};
        postObj.form = this.state.form;
        postObj.elements = this.state.elements;
        postObj.values = valuesToPost;
        const stringToSend = JSON.stringify({
            Form: this.state.form,
            Elements: this.state.elements,
            Values: valuesToPost
        });

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
                this.openModal();
            }
        });
    }

    valueChanged(elementId, newVal){
        let values = this.state.values;
        values[elementId] = newVal;
        this.setState({values: values}); 
    }

    render() {
        let elementsList = [];
        for(let key in this.state.elements){
            if(this.state.elements.hasOwnProperty(key)){
                elementsList.push(
                    <Element ref={"elem"+key} key={"element"+key} content={this.state.elements[key]} valueChanged={this.valueChanged.bind(this)}/>
                )
            }
        }

        let returnButton = null;
        if (this.props.user.userId)
        {
            returnButton = (<input type="button" 
                                    id="returnButton"
                                    onClick={this.handleReturn}
                                    value="Back to Project"
                                    className="btn btn-default" />);
        }

        let submitButton = null;
        if (!this.props.user.userId) {
            submitButton =
                <input type="button"
                    id="formButton"
                    onClick={this.handleSubmit}
                    value="Submit"
                    className="btn center-block btn-primary" />
        }
        
        return (
            <div id="elemContainer">
                <h1 className="text-center" key="h1">{this.state.form.formName}</h1>
                <div className="form-group">
                    {elementsList}

                    {submitButton}

                    {returnButton}

                    <Modal isOpen={this.state.isOpen} onRequestHide={this.redirectToHome}>
                        <ModalHeader>
                            <ModalClose onClick={this.hideModal}/>
                            <ModalTitle>Form has been submitted</ModalTitle>
                        </ModalHeader>
                        <ModalBody>
                            <button className='btn btn-default' onClick={this.redirectToHome}>
                                Close
                            </button>
                        </ModalBody>
                    </Modal>
                </div>
            </div>

        );
    };
}

export default  Form;