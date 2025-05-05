import { Alert, Button, Col, Container, Form, Modal, Row, Spinner } from 'react-bootstrap';
import FormField from '../../../core/components/FormField';
import SuccessModal from '../../../core/components/SuccessModal';
import { useRegisterModalViewModel } from './RegisterModalViewModel';

interface RegisterModalProps {
	show: boolean;
	onClose: () => void;
}

const RegisterModalView: React.FC<RegisterModalProps> = ({ show, onClose }) => {
	const vm = useRegisterModalViewModel(onClose);

	return (
		<>
			<Modal show={show} onHide={vm.handleClose} centered size='lg'>
				<Modal.Header closeButton>
					<Modal.Title>Rejestracja</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					{vm.error && (
						<Alert variant='danger' className='mb-3'>
							{vm.error}
						</Alert>
					)}
					<Form>
						<Row>
							<Col>
								<FormField
									label='Wprowadź email'
									type='email'
									floatingLabel={true}
									value={vm.email}
									onChange={e => vm.setEmail(e.target.value)}
									isInvalid={!!vm.emailError}
									feedback={vm.emailError}
								/>
							</Col>
							<Col>
								<FormField
									label='Potwierdź email'
									type='email'
									floatingLabel={true}
									value={vm.confirmEmail}
									onChange={e => vm.setConfirmEmail(e.target.value)}
									isInvalid={!!vm.emailError}
								/>
							</Col>
						</Row>
						<Row>
							<Col>
								<FormField
									label='Wprowadź hasło'
									type='password'
									floatingLabel={true}
									value={vm.password}
									onChange={e => vm.setPassword(e.target.value)}
									isInvalid={!!vm.passwordError}
									feedback={vm.passwordError}
								/>
							</Col>
							<Col>
								<FormField
									label='Potwierdź hasło'
									type='password'
									floatingLabel={true}
									value={vm.confirmPassword}
									onChange={e => vm.setConfirmPassword(e.target.value)}
									isInvalid={!!vm.passwordError}
								/>
							</Col>
						</Row>
						<Row>
							<Col>
								<FormField
									label='Imię'
									type='text'
									floatingLabel={true}
									value={vm.firstName}
									onChange={e => vm.setFirstName(e.target.value)}
									isInvalid={!!vm.firstNameError}
									feedback={vm.firstNameError}
								/>
							</Col>
							<Col>
								<FormField
									label='Nazwisko'
									type='text'
									floatingLabel={true}
									value={vm.lastName}
									onChange={e => vm.setLastName(e.target.value)}
									isInvalid={!!vm.lastNameError}
									feedback={vm.lastNameError}
								/>
							</Col>
						</Row>
						<Row>
							<Col md='7'>
								<FormField
									label='Ulica'
									type='text'
									floatingLabel={true}
									value={vm.street}
									onChange={e => vm.setStreet(e.target.value)}
									isInvalid={!!vm.streetError}
									feedback={vm.streetError}
								/>
							</Col>
							<Col>
								<FormField
									label='Nr. budynku'
									type='text'
									floatingLabel={true}
									value={vm.building}
									onChange={e => vm.setBuilding(e.target.value)}
									isInvalid={!!vm.buildingError}
									feedback={vm.buildingError}
								/>
							</Col>
							<Col>
								<FormField
									label='Nr. lokalu'
									type='text'
									floatingLabel={true}
									value={vm.room}
									onChange={e => vm.setRoom(e.target.value)}
									isInvalid={!!vm.roomError}
									feedback={vm.roomError}
								/>
							</Col>
						</Row>
						<Row>
							<Col md='4'>
								<FormField
									label='Kod pocztowy'
									type='text'
									floatingLabel={true}
									value={vm.code}
									onChange={e => vm.setAndCheckCode(e.target.value)}
									isInvalid={!!vm.codeError}
									feedback={vm.codeError}
								/>
							</Col>
							<Col>
								<FormField
									label='Miasto'
									type='text'
									floatingLabel={true}
									value={vm.post}
									onChange={e => vm.setPost(e.target.value)}
									isInvalid={!!vm.postError}
									feedback={vm.postError}
								/>
							</Col>
						</Row>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					{vm.loading && (
						<Container className='d-flex justify-content-center align-items-center'>
							<Spinner animation='border' />
						</Container>
					)}
					<Button variant='secondary' onClick={vm.handleClose}>
						Zamknij
					</Button>
					<Button variant='primary' onClick={vm.handleRegister}>
						Zarejestruj
					</Button>
				</Modal.Footer>
			</Modal>

			{vm.showSuccessModal && (
				<SuccessModal show={vm.showSuccessModal} onClose={vm.handleSuccessClose} title='Rejestracja pomyślna' />
			)}
		</>
	);
};

export default RegisterModalView;
