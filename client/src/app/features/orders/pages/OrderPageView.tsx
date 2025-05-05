import { Alert, Button, Container, Table } from 'react-bootstrap';
import { useOrderPageViewModel } from './OrderPageViewModel';
import InfoModal from '../../../core/components/InfoModal';
import ConfirmModal from '../../../core/components/ConfirmModal';

const OrderPageView: React.FC = () => {
	const vm = useOrderPageViewModel();

	if (vm.loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<div>Ładownaie...</div>
			</Container>
		);
	}

	if (vm.error) {
		return (
			<Container>
				<Alert variant='danger'>{vm.error}</Alert>
			</Container>
		);
	}

	return (
		<>
			<Container className='mb-5 content-container'>
				{vm.order?.orderItems && vm.order.orderItems.items.length > 0 ? (
					<>
						<h1>Zamówienie</h1>
						{!vm.isLoggedIn && (
							<Alert variant='warning' className='text-center mt-4'>
								Jesteś niezalogowany. Zaloguj się aby Twoje dane były automatycznie uzupełnione.
							</Alert>
						)}
						<Table bordered className='mt-4'>
							<thead>
								<tr>
									<th colSpan={2} className='text-center'>
										Wydarzenie
									</th>
									<th className='text-center'>Ilość</th>
									<th className='text-center'>Akcja</th>
								</tr>
							</thead>
							<tbody>
								{vm.order.orderItems.items.map(item => (
									<tr key={item.eventId}>
										<td className='text-center align-middle'>{item.eventName}</td>
										<td className='text-center align-middle'>{item.price}</td>
										<td className='text-center align-middle'>{item.quantity}</td>
										<td className='justify-content-center d-flex gap-2 align-middle'>
											<Button
												onClick={() => vm.changeQuantity(item.orderId, item.id, 1)}
												variant='success'
												className='order-btn'>
												Dodaj
											</Button>
											<Button
												onClick={() => vm.changeQuantity(item.orderId, item.id, -1)}
												variant='danger'
												className='order-btn'>
												Usuń
											</Button>
										</td>
									</tr>
								))}
							</tbody>
						</Table>
					</>
				) : (
					<Alert variant='info' className='text-center mt-4'>
						Twoje zamówienie jest puste. Dodaj wydarzenia do zamówienia, aby je zobaczyć.
					</Alert>
				)}
			</Container>
			<Container>
				{vm.order?.orderItems && vm.order.orderItems.items.length > 0 ? (
					<div className='d-flex justify-content-between m-2'>
						<h2>Suma: {vm.getOrderPrice()} </h2>
						<Button
							variant='info'
							className='order-btn'
							onClick={() => {
								vm.completeOrder();
							}}>
							Potwierdź zamówienie
						</Button>
					</div>
				) : (
					<></>
				)}
			</Container>
			{vm.showInfoModal && vm.info && (
				<InfoModal show={!!vm.info} onClose={vm.handleCloseInfo} title='Info' message={vm.info} />
			)}
			{vm.showConfirmationModal && vm.confirmationQuestion && (
				<ConfirmModal
					show={vm.showConfirmationModal}
					onClose={vm.handleConfirmationClose}
					title='Confirmation'
					message={vm.confirmationQuestion}
					onConfirm={vm.handleConfirmationAccept}
				/>
			)}
		</>
	);
};

export default OrderPageView;
