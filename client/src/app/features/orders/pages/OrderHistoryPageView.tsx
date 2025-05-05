import { Alert, Container, Spinner, Table } from 'react-bootstrap';
import { useOrderHistoryPageViewModel } from './OrderHistoryPageViewModel';

const OrderHistoryPageView = () => {
	const vm = useOrderHistoryPageViewModel();
	if (vm.loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		);
	}

	if (vm.error) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Alert variant='danger'>{vm.error}</Alert>
			</Container>
		);
	}

	return (
		<div>
			<Container className='mb-5 content-container'>
				<h1>Historia zamówień</h1>
				<div className='list-container'>
					<div className='d-flex gap-5 justify-content-center flex-wrap'>
						<Table bordered>
							<thead>
								<tr>
									<th className='text-center'>Nazwa wydarzenia</th>
									<th className='text-center'>Ilość</th>
									<th className='text-center'>Cena jednostkowa</th>
									<th className='text-center'>Data zamówienia</th>
								</tr>
							</thead>
							<tbody>
								{vm.orders.map(order =>
									order.orderItems.items.map(item => (
										<tr key={item.id}>
											<td className='text-center align-middle'>{item.eventName}</td>
											<td className='text-center align-middle'>{item.quantity}</td>
											<td className='text-center align-middle'>{item.price}</td>
											<td className='text-center align-middle'>{vm.dateToLocaleString(order.createdAt)}</td>
										</tr>
									))
								)}
							</tbody>
						</Table>
					</div>
				</div>
			</Container>
		</div>
	);
};

export default OrderHistoryPageView;
