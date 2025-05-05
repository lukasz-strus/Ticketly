import { Alert, Button, Container, Spinner, Table } from 'react-bootstrap';
import { useManageEventsViewModel } from './ManageEventsViewModel';
import EventFormModalView from '../components/EventFormModalView';
import TicketPoolsModalView from '../components/TicketPoolsModalView';

const ManageEventsView: React.FC = () => {
	const vm = useManageEventsViewModel();
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
		<>
			<Container className='d-flex justify-content-center align-items-center mt-5'>
				<h1>Zarządzaj wydarzeniami</h1>
			</Container>
			<Container className='d-flex justify-content-center align-items-center mt-5'>
				<Button variant='success' className='order-btn' onClick={vm.handleOnCreateEvent}>
					Dodaj nowe wydarzenie
				</Button>
			</Container>
			<Container className='content-container'>
				<div className='list-container'>
					<div className='d-flex gap-5 justify-content-center flex-wrap'>
						<Table bordered>
							<thead>
								<tr>
									<th className='text-center'>Nazwa</th>
									<th className='text-center'>Opis</th>
									<th className='text-center'>Kategoria</th>
									<th className='text-center'>Data</th>
									<th className='text-center'>Akcja</th>
								</tr>
							</thead>
							<tbody>
								{vm.events.map(event => (
									<tr key={event.id}>
										<td className='text-center align-middle'>{event.name}</td>
										<td className='text-center align-middle'>{event.description}</td>
										<td className='text-center align-middle'>{event.categoryName}</td>
										<td className='text-center align-middle'>{vm.dateToLocaleString(event.date)}</td>
										<td className='text-center align-middle justify-content-center d-flex gap-2'>
											<Button variant='info' className='order-btn' onClick={() => vm.handleTicketPoolsEvent(event.id)}>
												Pule biletowe
											</Button>
											<Button variant='warning' className='order-btn' onClick={() => vm.handleOnEditEvent(event.id)}>
												Edytuj
											</Button>
											<Button variant='danger' className='order-btn' onClick={() => vm.handleOnDeleteEvent(event.id)}>
												Usuń
											</Button>
										</td>
									</tr>
								))}
							</tbody>
						</Table>
					</div>
				</div>
			</Container>
			{vm.showEventFormModal && (
				<EventFormModalView
					show={vm.showEventFormModal}
					onClose={vm.handleCloseEventForm}
					eventId={vm.selectedEventId}
				/>
			)}
			{vm.showTicketPoolsModal && (
				<TicketPoolsModalView
					show={vm.showTicketPoolsModal}
					onClose={vm.handleCloseTicketPoolsModal}
					eventId={vm.selectedEventId}
				/>
			)}
		</>
	);
};

export default ManageEventsView;
