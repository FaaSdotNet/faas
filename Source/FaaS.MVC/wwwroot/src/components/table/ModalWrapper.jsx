import React, {Component} from "react";
import {Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter} from 'react-modal-bootstrap';

export class ModalWrapper extends Component {

	/**
	 *
	 * @param props
	 */
	constructor(props)
	{
		super(props);
		this.openModal = this.openModal.bind(this);
		this.hideModal = this.hideModal.bind(this);

		this.handleClose = this.props.handleClose;

		this.state = {
			isOpen: this.props.open
		};
	}

	openModal()
	{
		this.setState({
			isOpen: true
		});
	};

	hideModal()
	{
		this.setState({
			isOpen: false
		});
	};

	componentWillUpdate(nextProps, nextState)
	{
		if(this.props.open && this.state.isOpen != true){
			this.openModal();
		}
	}

	componentDidMount(){
		if(this.props.open && this.state.isOpen != true){
			this.openModal();
		}
	}


	render()
	{
		return (
			<div>
				<Modal isOpen={this.state.isOpen} onRequestHide={this.hideModal}>
					<ModalHeader>
						<ModalClose onClick={ () =>
						{
							this.hideModal();
						}}/>
						<ModalTitle>{this.props.title}</ModalTitle>
					</ModalHeader>
					<ModalBody>
						{this.props.children}
					</ModalBody>
					<ModalFooter>

						<button className='btn btn-default' onClick={ () =>
						{
							this.hideModal();
						}}>
							Close
						</button>
					</ModalFooter>
				</Modal>
			</div>
		);
	}

}

export default ModalWrapper;