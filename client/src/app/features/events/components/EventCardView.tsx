import { Badge, Button, Card } from 'react-bootstrap';
import React, { FormEvent, useEffect, useState } from 'react';
import { Event, TicketPool, TicketPools } from '../../../core/contracts/Event';

interface EventCardViewProps {
	event: Event;
	onClick: (eventId: string) => void;
	handleOnAddToOrder: (eventId: string) => void;
}

const EventCardView: React.FC<EventCardViewProps> = ({ event, onClick, handleOnAddToOrder }) => {
	const [isAvailable, setIsAvailable] = useState<boolean>(false);
	const handleEventClick = (formEvent: FormEvent) => {
		formEvent.preventDefault();

		if (!event) return;

		onClick(event.id);
	};

	function dateToLocaleString(date: string): string {
		return new Date(date).toLocaleDateString() + 'r.';
	}

	const getPrice = (ticketPools: TicketPools): string => {
		if (ticketPools.items.length > 0) {
			const availableTicketPools = ticketPools.items.filter(ticketPool => ticketPool.availableTickets > 0);
			if (availableTicketPools.length > 0) {
				const sortedTicketPools = availableTicketPools.sort((a, b) => a.priceAmount - b.priceAmount);
				return `${sortedTicketPools[0].price}`;
			}
		}
		return 'N/A';
	};

	useEffect(() => {
		function checkTickets(ticketPools: TicketPools): boolean {
			return ticketPools.items.some((ticketPool: TicketPool) => {
				const now = new Date();
				const startDate = new Date(ticketPool.startDate);
				const endDate = new Date(ticketPool.endDate);
				return ticketPool.availableTickets > 0 && now >= startDate && now <= endDate;
			});
		}
		const available = checkTickets(event.ticketPools);
		setIsAvailable(available);
	}, [event.ticketPools]);

	return (
		<Card className='event-card' onClick={handleEventClick}>
			<Card.Img
				src={event.imageUrl}
				alt={'Obraz przedstawiający wydarzenie: ' + event.name + '.'}
				className='event-card-img'
			/>
			<Card.Body>
				<Card.Title className='fs-3 text-center'>{event.name}</Card.Title>
				<Card.Text className='fs-5 text-center'>
					{isAvailable && <Badge bg='success'>W sprzedaży</Badge>}
					{!isAvailable && <Badge bg='danger'>Wyprzedane</Badge>}
				</Card.Text>
				<Card.Text className='fs-5 text-center'>
					{dateToLocaleString(event.date)} {event.locationPost}
				</Card.Text>
				{isAvailable && (
					<Card.Footer className='fs-4 text-center'>
						<Button variant='warning' className='fs-7 ticket-btn' onClick={() => handleOnAddToOrder(event.id)}>
							<strong className='text-dark'>Kup bilet {getPrice(event?.ticketPools)}</strong>
						</Button>
					</Card.Footer>
				)}
			</Card.Body>
		</Card>
	);
};

export default EventCardView;
