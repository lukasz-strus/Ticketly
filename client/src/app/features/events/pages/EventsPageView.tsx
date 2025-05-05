import { Alert, Container, Spinner } from 'react-bootstrap';
import { useEventsPageViewModel } from './EventsPageViewModel';
import EventCardView from '../components/EventCardView';

const EventsPageView: React.FC = () => {
	const vm = useEventsPageViewModel();

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
			<Container className='mb-5 content-container'>
				<h1>{vm.categoryId ? vm.categoryName : 'Wydarzenia'}</h1>
				<div className='list-container'>
					<div className='d-flex gap-5 justify-content-center flex-wrap'>
						{vm.events.map(event => (
							<EventCardView
								key={event.id}
								event={event}
								onClick={eventId => vm.navigate(`/events/${eventId}`)}
								handleOnAddToOrder={vm.handleOnAddToOrder}
							/>
						))}
					</div>
				</div>
			</Container>
		</>
	);
};

export default EventsPageView;
