import React, {Component} from "react";
import {Modal, ModalHeader, ModalTitle, ModalClose, ModalBody, ModalFooter} from 'react-modal-bootstrap';

export class ButtonDelete extends Component {

	/**
	 *
	 * @param props
	 * @property [Object] item
	 * @property [Function] handleDelete
	 */
	constructor(props)
	{
		super(props);
		this.openModal = this.openModal.bind(this);
		this.hideModal = this.hideModal.bind(this);

		this.handleDelete = this.props.handleDelete;

		this.state = {
			isOpen: false
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

	render()
	{
		return (

			<div>
				<button type="button" className="btn btn-default btn-md" onClick={ () =>
				{
					this.openModal();
				}}>
					<span style={{fontSize: 1.5 + 'em'}} className="glyphicon glyphicon-trash" aria-hidden="true"/>
				</button>
				<Modal isOpen={this.state.isOpen} onRequestHide={this.hideModal}>
					<ModalHeader>
						<ModalClose onClick={ () =>
						{
							this.hideModal();
						}}/>
						<ModalTitle>Confirm Delete</ModalTitle>
					</ModalHeader>
					<ModalBody>
						<p>Are you sure you want to delete?</p>
					</ModalBody>
					<ModalFooter>
						<button onClick={() =>
						{
							this.props.handleDelete(this.props.item.id);
							this.hideModal();
						} }
								className='btn btn-primary'>
							Delete
						</button>
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

export default ButtonDelete;