import React from 'react';
import { Button, Modal } from 'react-bootstrap';

interface ConfirmModalProps {
	show: boolean;
	onClose: () => void;
	onConfirm: () => void;
	title: string;
	message?: string;
}

const ConfirmModal: React.FC<ConfirmModalProps> = ({ show, onClose, onConfirm, title, message }) => {
	return (
		<Modal show={show} onHide={onClose} centered>
			<Modal.Header className='bg-warning fs-6' closeButton>
				<Modal.Title className='fs-4'>{title}</Modal.Title>
			</Modal.Header>
			{message && (
				<Modal.Body className='fs-5'>
					<p>{message}</p>
				</Modal.Body>
			)}
			<Modal.Footer className=''>
				<Button variant='warning' onClick={onConfirm} className='fs-6'>
					Tak
				</Button>
				<Button variant='info' onClick={onClose} className='fs-6'>
					Nie
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default ConfirmModal;
