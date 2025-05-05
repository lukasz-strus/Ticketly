import { Alert, Button, Container, Form, Modal, Spinner } from 'react-bootstrap';
import FormField from '../../../core/components/FormField';
import SuccessModal from '../../../core/components/SuccessModal';
import RegisterModalView from './RegisterModalView';
import { useLoginModalViewModel } from './LoginModalViewModel';

interface LoginModalProps {
	show: boolean;
	onClose: () => void;
}

const LoginModalView: React.FC<LoginModalProps> = ({ show, onClose }) => {
	const vm = useLoginModalViewModel(onClose);

	return (
		<>
			<Modal show={show} onHide={vm.handleClose} centered>
				<Modal.Header closeButton>
					<Modal.Title>Logowanie</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					{vm.error && (
						<Alert variant='danger' className='mb-3'>
							{vm.error}
						</Alert>
					)}
					<Form>
						<FormField
							label='Email'
							type='email'
							floatingLabel={true}
							value={vm.email}
							onChange={e => vm.setEmail(e.target.value)}
							isInvalid={!!vm.emailError}
							feedback={vm.emailError}
						/>
						<FormField
							label='Hasło'
							type='password'
							floatingLabel={true}
							value={vm.password}
							onChange={e => vm.setPassword(e.target.value)}
							isInvalid={!!vm.passwordError}
							feedback={vm.passwordError}
						/>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					{vm.loading && (
						<Container className='d-flex justify-content-center align-items-center'>
							<Spinner animation='border' />
						</Container>
					)}
					<p className='text-muted'>
						Nie masz konta?{' '}
						<Button variant='link' onClick={() => vm.setShowRegisterModal(true)}>
							Zarejestruj się
						</Button>
					</p>
					<Button variant='secondary' onClick={vm.handleClose}>
						Zamknij
					</Button>
					<Button variant='primary' onClick={vm.handleLogin}>
						Zaloguj
					</Button>
				</Modal.Footer>

				{vm.showRegisterModal && (
					<RegisterModalView show={vm.showRegisterModal} onClose={() => vm.setShowRegisterModal(false)} />
				)}

				{vm.showSuccessModal && (
					<SuccessModal show={vm.showSuccessModal} onClose={vm.handleSuccessClose} title='Logowanie pomyślne' />
				)}
			</Modal>
		</>
	);
};

export default LoginModalView;
