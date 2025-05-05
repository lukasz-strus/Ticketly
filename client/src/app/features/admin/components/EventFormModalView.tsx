import { Alert, Button, Col, Container, Form, Modal, Row, Spinner } from 'react-bootstrap';
import { useEventFormModalViewModel } from './EventFormModalViewModel';
import FormField from '../../../core/components/FormField';

interface EventFormModalViewProps {
	show: boolean;
	onClose: () => void;
	eventId: string | null;
}

const EventFormModalView: React.FC<EventFormModalViewProps> = ({ show, onClose, eventId }) => {
	const vm = useEventFormModalViewModel(onClose, eventId);

	return (
		<>
			<Modal show={show} onHide={vm.handleClose} centered size='lg'>
				<Modal.Header closeButton>
					<Modal.Title>{eventId ? 'Edytuj wydarzenie' : 'Utwórz wydarzenie'}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					{vm.error && (
						<Alert variant='danger' className='mb-3'>
							{vm.error}
						</Alert>
					)}
					<Form>
						<Row>
							<Col md={6}>
								<FormField
									label='Nazwa'
									type='text'
									floatingLabel={true}
									value={vm.name}
									onChange={e => vm.setName(e.target.value)}
									isInvalid={!!vm.nameError}
									feedback={vm.nameError}
								/>
							</Col>
							<Col md={3}>
								<FormField
									label='Kategoria'
									type='select'
									floatingLabel={true}
									value={vm.selectedCategory}
									options={vm.categories.map(category => ({
										value: category,
										label: category,
									}))}
									onChange={e => vm.setSelectedCategory(e.target.value)}
								/>
							</Col>
							<Col md={3}>
								<FormField
									label='Data'
									type='date'
									floatingLabel={true}
									value={vm.date}
									onChange={e => vm.setDate(e.target.value)}
									isInvalid={!!vm.dateError}
									feedback={vm.dateError}
								/>
							</Col>
						</Row>
						<Row>
							<Col>
								<FormField
									label='Opis'
									type='text'
									as='textarea'
									floatingLabel={true}
									value={vm.description || ''}
									onChange={e => vm.setDescription(e.target.value)}
									isInvalid={!!vm.descriptionError}
									feedback={vm.descriptionError}
								/>
							</Col>
						</Row>
						<Row>
							<Col md={7}>
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
							<Col md={4}>
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
						<Row>
							<Col>
								<FormField
									label='URL zdjęcia'
									type='text'
									floatingLabel={true}
									value={vm.imgUrl}
									onChange={e => vm.setImgUrl(e.target.value)}
									isInvalid={!!vm.imgUrlError}
									feedback={vm.imgUrlError}
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
					<Button variant='primary' onClick={vm.handleSave}>
						Zapisz
					</Button>
				</Modal.Footer>
			</Modal>
		</>
	);
};

export default EventFormModalView;
