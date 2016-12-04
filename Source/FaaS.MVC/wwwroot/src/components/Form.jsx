import React from 'react';
import Element from './form/Element.jsx'
import MySubmit from "./form/MySubmit.jsx"
import { Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter } from 'react-modal-bootstrap';

class Form extends React.Component {
    constructor(props) {
        super(props);
        this.openModal = this.openModal.bind(this);

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
        document.location.href = "/#/index";
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

    handleSubmit(e) {
        var valuesToPost = [];
        var i = 0;
        for(var key in this.state.elements){
            valuesToPost[i] = this.state.values[this.state.elements[key].id]
            i++;
        }

        var incomplete = false;
        for(var key in this.state.elements){
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
        
        var postObj = {};
        postObj.form = this.state.form;
        postObj.elements = this.state.elements;
        postObj.values = valuesToPost;
        var stringToSend = JSON.stringify({
            Form: this.state.form,
            Elements: this.state.elements,
            Values: valuesToPost
        });

        var result = fetch('/api/v1.0/form', {
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
        var values = this.state.values;
        values[elementId] = newVal;
        this.setState({values: values}); 
    }

    render() {
        var elementsList = [];
        for(var key in this.state.elements){
            if(this.state.elements.hasOwnProperty(key)){
                elementsList.push(<li key={key}><Element ref={"elem"+key} key={"element"+key} content={this.state.elements[key]} valueChanged={this.valueChanged.bind(this)}/></li>)
            }
        }
        
        return (
            <div id="elemContainer">
                <h1 key="h1">{this.state.form.formName}</h1>
                {elementsList}
                <MySubmit onClick={this.handleSubmit.bind(this)} id="formButton" value="Submit"/>

                <Modal isOpen={this.state.isOpen} onRequestHide={this.hideModal}>
                    <ModalHeader>
                        <ModalClose onClick={this.hideModal}/>
                        <ModalTitle>Form has been submitted, you can die in peace now</ModalTitle>
                    </ModalHeader>
                    <ModalBody>
                        <button className='btn btn-default' onClick={this.redirectToHome}>
                            Close
                        </button>
                    </ModalBody>
                </Modal>
            </div>

        );
    };
}

export default  Form;