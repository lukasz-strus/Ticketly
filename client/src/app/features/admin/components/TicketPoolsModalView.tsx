import { Alert, Button, Col, Container, Form, Modal, Row, Table } from 'react-bootstrap';
import { useTicketPoolsModalViewModel } from './TicketPoolsModalViewModel';
import FormField from '../../../core/components/FormField';

interface TicketPoolsModalView {
	show: boolean;
	onClose: () => void;
	eventId: string | null;
}

const TicketPoolsModalView: React.FC<TicketPoolsModalView> = ({ show, onClose, eventId }) => {
	const vm = useTicketPoolsModalViewModel(onClose, eventId);

	return (
		<>
			<Modal show={show} onHide={vm.handleClose} centered size='lg'>
				<Modal.Header closeButton>
					<Modal.Title>Pule biletowe</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					{vm.error && (
						<Alert variant='danger' className='mb-3'>
							{vm.error}
						</Alert>
					)}
					<Table bordered hover className='mt-4'>
						<thead>
							<tr>
								<th className='text-center'>Cena</th>
								<th className='text-center'>Liczba biletów</th>
								<th className='text-center'>Akcja</th>
							</tr>
						</thead>
						<tbody>
							{vm.ticketPools.map(ticketPool => (
								<tr key={ticketPool.id}>
									<td className='text-center align-middle'>{ticketPool.price}</td>
									<td className='text-center align-middle'>{ticketPool.availableTickets}</td>
									<td className='text-center align-middle'>
										<Button variant='danger' onClick={() => vm.handleOnDeleteTicketPool(ticketPool.id)}>
											Delete
										</Button>
									</td>
								</tr>
							))}
						</tbody>
					</Table>
					<Container className='d-flex justify-content-center align-items-center mt-5'>
						<Form>
							<Row>
								<Col>
									<FormField
										label='Cena'
										type='number'
										floatingLabel={true}
										value={vm.newTicketPoolPrice}
										onChange={e => vm.setAndCheckPrice(Number(e.target.value))}
										isInvalid={!!vm.newTicketPoolPriceError}
										feedback={vm.newTicketPoolPriceError}
									/>
								</Col>
								<Col>
									<FormField
										label='Waluta'
										type='string'
										floatingLabel={true}
										value='PLN'
										onChange={() => {}}
										disabled={true}
									/>
								</Col>
								<Col>
									<FormField
										label='Liczba biletów'
										type='number'
										floatingLabel={true}
										value={vm.newTicketPoolQuantity}
										onChange={e => vm.setAndCheckQuantity(Number(e.target.value))}
										isInvalid={!!vm.newTicketPoolQuantityError}
										feedback={vm.newTicketPoolQuantityError}
									/>
								</Col>
							</Row>
							<Row>
								<Col>
									<FormField
										label='Data rozpoczęcia'
										type='date'
										floatingLabel={true}
										value={vm.newStartDate}
										onChange={e => vm.setAndCheckStartDate(e.target.value)}
										isInvalid={!!vm.newStartDateError}
										feedback={vm.newStartDateError}
									/>
								</Col>
								<Col>
									<FormField
										label='Data zakończenia'
										type='date'
										floatingLabel={true}
										value={vm.newEndDate}
										onChange={e => vm.setAndCheckEndDate(e.target.value)}
										isInvalid={!!vm.newEndDateError}
										feedback={vm.newEndDateError}
									/>
								</Col>
								<Col className='d-flex align-items-center justify-content-center'>
									<Button className='order-btn' variant='warning' onClick={vm.handleOnCreateTicketPool}>
										Dodaj pulę biletową
									</Button>
								</Col>
							</Row>
						</Form>
					</Container>
				</Modal.Body>
			</Modal>
		</>
	);
};

export default TicketPoolsModalView;
