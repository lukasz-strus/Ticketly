import React from 'react';
import { Button, Modal } from 'react-bootstrap';

interface SuccessModalProps {
	show: boolean;
	onClose: () => void;
	title: string;
	message?: string;
}

const SuccessModal: React.FC<SuccessModalProps> = ({ show, onClose, title, message }) => {
	return (
		<Modal show={show} onHide={onClose} centered>
			<Modal.Header className='bg-success fs-6' closeButton>
				<Modal.Title className='fs-4'>{title}</Modal.Title>
			</Modal.Header>
			{message && (
				<Modal.Body className='fs-5'>
					<p>{message}</p>
				</Modal.Body>
			)}
			<Modal.Footer className=''>
				<Button variant='success' onClick={onClose} className='fs-6'>
					Ok
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default SuccessModal;
