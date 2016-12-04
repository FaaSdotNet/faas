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

		this.open = this.props.open;

		this.state = {
			isOpen: false
		};
	}

	openModal()
	{
		console.log("Open called!");

		this.setState({
			isOpen: true
		});

		this.props.open.open = true;
	};

	hideModal()
	{
		this.setState({
			isOpen: false
		});
		this.props.open.open = false;

	};

	componentWillUpdate(nextProps, nextState)
	{
		console.log("[State] ", this.props.open.open, this.state.isOpen);
		if(this.props.open.open && this.state.isOpen == false) {
			console.log("Open should be called");
			this.openModal();
		}
	}


	render()
	{
		return (
			<div>
				<Modal isOpen={this.props.open.open} onRequestHide={this.hideModal}>
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