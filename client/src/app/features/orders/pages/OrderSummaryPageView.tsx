import { Alert, Button, Col, Container, Form, Row, Table } from 'react-bootstrap';
import { useOrderSummaryPageViewModel } from './OrderSummaryPageViewModel';
import SuccessModal from '../../../core/components/SuccessModal';
import FormField from '../../../core/components/FormField';

const OrderSummaryPageView: React.FC = () => {
	const vm = useOrderSummaryPageViewModel();
	return (
		<>
			<Container className='mt-4'>
				{!vm.isLoggedIn && (
					<Alert variant='warning' className='text-center mt-4'>
						Nie jesteś zalogowany. Zaloguj się, aby Twoje dane były automatycznie uzupełnione.
					</Alert>
				)}
				<h1>Podsumowanie</h1>
				<Form className='mt-4'>
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
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
								disabled={vm.isLoggedIn}
							/>
						</Col>
					</Row>
				</Form>
				<div className='d-flex justify-content-between align-items-center mt-4'>
					<div>
						{vm.order?.orderItems && vm.order.orderItems.items.length > 0 ? (
							<Table bordered className='mt-4'>
								<thead>
									<tr>
										<th colSpan={2} className='text-center'>
											Wydarzenie
										</th>
										<th className='text-center'>Ilość</th>
									</tr>
								</thead>
								<tbody>
									{vm.order.orderItems.items.map(item => (
										<tr key={item.eventId}>
											<td className='text-center align-middle'>{item.eventName}</td>
											<td className='text-center align-middle'>{item.price}</td>
											<td className='text-center align-middle'>{item.quantity}</td>
										</tr>
									))}
								</tbody>
							</Table>
						) : (
							<Alert variant='warning' className='text-center mt-4'>
								Nie dodano żadnych biletów do zamówienia.
							</Alert>
						)}
						<h4>Suma {vm.getOrderPrice()} </h4>
					</div>

					<Button
						variant='warning'
						className='order-btn fs-5'
						onClick={() => {
							vm.completeOrder();
						}}>
						Potwierdź zakup
					</Button>
				</div>
			</Container>

			{vm.showInfoModal && (
				<SuccessModal show={vm.showInfoModal} onClose={vm.handleSuccessClose} title={vm.info + ''} />
			)}
		</>
	);
};

export default OrderSummaryPageView;
