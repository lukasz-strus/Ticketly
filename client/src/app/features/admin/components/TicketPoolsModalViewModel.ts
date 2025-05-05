import { useEffect, useState } from 'react';
import { Event, TicketPool, TicketPoolRequest } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';
import { Currency } from '../../../core/enums/Currency';

const eventService = EventService.getInstance();

export const useTicketPoolsModalViewModel = (onClose: () => void, eventId: string | null) => {
	const [event, setEvent] = useState<Event | null>(null);
	const [ticketPools, setTicketPools] = useState<TicketPool[]>([]);
	const [newTicketPoolQuantity, setNewTicketPoolQuantity] = useState<number>(0);
	const [newTicketPoolQuantityError, setNewTicketPoolQuantityError] = useState<string | null>(null);
	const [newTicketPoolPrice, setNewTicketPoolPrice] = useState<number>(0);
	const [newTicketPoolPriceError, setNewTicketPoolPriceError] = useState<string | null>(null);
	const [newStartDate, setNewStartDate] = useState('');
	const [newStartDateError, setNewStartDateError] = useState<string | null>(null);

	const [newEndDate, setNewEndDate] = useState('');
	const [newEndDateError, setNewEndDateError] = useState<string | null>(null);

	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);

	const validateForm = () => {
		let hasError = false;

		setNewTicketPoolQuantityError(null);
		setNewTicketPoolPriceError(null);
		setNewStartDateError(null);
		setNewEndDateError(null);
		setError(null);

		if (newTicketPoolQuantity < 0) {
			setNewTicketPoolQuantityError('Quantity cannot be negative.');
			hasError = true;
		}

		if (newTicketPoolPrice < 0) {
			setNewTicketPoolPriceError('Price cannot be negative.');
			hasError = true;
		}

		if (!newStartDate) {
			setNewStartDateError('Start date is required.');
			hasError = true;
		}

		if (!newEndDate) {
			setNewEndDateError('End date is required.');
			hasError = true;
		}

		if (newStartDate > newEndDate) {
			setNewStartDateError('Start date cannot be later than the end date.');
			setNewEndDateError('End date cannot be earlier than the start date.');
			hasError = true;
		}

		if (!event) {
			setError('Event not found.');
			hasError = true;
			return false;
		}

		if (newStartDate > event?.date) {
			setNewStartDateError('Start date cannot be later than the event date.');
			hasError = true;
		}

		if (newEndDate > event?.date) {
			setNewEndDateError('End date cannot be later than the event date.');
			hasError = true;
		}

		if (hasError) {
			setError('Please fix the errors before submitting.');
			return false;
		}
	};

	const setAndCheckPrice = (price: number) => {
		if (price < 0) {
			setNewTicketPoolPriceError('Price cannot be negative.');
			return;
		}
		setNewTicketPoolPriceError(null);
		setNewTicketPoolPrice(price);
	};

	const setAndCheckQuantity = (quantity: number) => {
		if (quantity < 0) {
			setNewTicketPoolQuantityError('Quantity cannot be negative.');
			return;
		}
		setNewTicketPoolQuantityError(null);
		setNewTicketPoolQuantity(quantity);
	};

	const setAndCheckStartDate = (startDate: string) => {
		const eventDate = new Date(event?.date || '');
		const selectedStartDate = new Date(startDate);
		const selectedEndDate = new Date(newEndDate);
		if (selectedStartDate > eventDate) {
			setNewStartDateError('Start date cannot be later than the event date.');
			return;
		}
		if (selectedStartDate > selectedEndDate) {
			setNewStartDateError('Start date cannot be later than the end date.');
			return;
		}
		setNewStartDateError(null);
		setNewStartDate(startDate);
	};

	const setAndCheckEndDate = (endDate: string) => {
		const eventDate = new Date(event?.date || '');
		const selectedStartDate = new Date(newStartDate);
		const selectedEndDate = new Date(endDate);
		if (selectedEndDate < selectedStartDate) {
			setNewEndDateError('End date cannot be earlier than the start date.');
			return;
		}

		if (selectedEndDate > eventDate) {
			setNewEndDateError('End date cannot be earlier than the start date.');
			return;
		}
		setNewEndDateError(null);
		setNewEndDate(endDate);
	};

	const handleClose = () => {
		setNewTicketPoolQuantity(0);
		setNewTicketPoolPrice(0);
		setError(null);
		onClose();
	};

	const handleOnCreateTicketPool = async () => {
		const token = localStorage.getItem('authToken');
		if (!token) {
			setError('User is not authenticated.');
			return;
		}

		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		if (validateForm()) {
			return;
		}

		setLoading(true);
		setError(null);

		try {
			const newTicketPool: TicketPoolRequest = {
				availableTickets: newTicketPoolQuantity,
				price: newTicketPoolPrice,
				currencyId: Currency.PLN.id,
				startDate: newStartDate,
				endDate: newEndDate,
			};

			await eventService.createTicketPool(token, eventId, newTicketPool);
			const response = await eventService.getEvent(eventId);
			if (!response) {
				setError('Event not found.');
				return;
			}
			setEvent(response);
			setTicketPools(response.ticketPools.items);
		} catch (err) {
			if (err instanceof Error) {
				setError(err.message);
			} else {
				setError('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	const handleOnDeleteTicketPool = async (ticketPoolId: string) => {
		const token = localStorage.getItem('authToken');
		if (!token) {
			setError('User is not authenticated.');
			return;
		}
		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		if (!ticketPoolId) {
			setError('Ticket pool ID is not provided.');
			return;
		}
		setLoading(true);
		setError(null);

		try {
			await eventService.deleteTicketPool(token, eventId, ticketPoolId);
			setTicketPools(prevTicketPools => prevTicketPools.filter(ticketPool => ticketPool.id !== ticketPoolId));
		} catch (err) {
			if (err instanceof Error) {
				setError(err.message);
			} else {
				setError('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	const fetchTicketPools = async () => {
		setLoading(true);
		setError(null);

		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		try {
			const response = await eventService.getEvent(eventId);
			if (!response) {
				setError('Event not found.');
				return;
			}
			setEvent(response);
			setTicketPools(response.ticketPools.items);
		} catch (err) {
			if (err instanceof Error) {
				setError(err.message);
			} else {
				setError('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		const token = localStorage.getItem('authToken');
		if (!token) {
			setError('User is not authenticated.');
			return;
		}
		fetchTicketPools();
	}, [eventId]);

	return {
		ticketPools,
		newTicketPoolQuantity,
		setNewTicketPoolQuantity,
		newTicketPoolPrice,
		setNewTicketPoolPrice,
		loading,
		error,
		handleClose,
		setAndCheckPrice,
		setAndCheckQuantity,
		newTicketPoolQuantityError,
		newTicketPoolPriceError,
		newStartDate,
		setNewStartDate,
		newStartDateError,
		newEndDate,
		setNewEndDate,
		newEndDateError,
		handleOnDeleteTicketPool,
		setAndCheckStartDate,
		setAndCheckEndDate,
		handleOnCreateTicketPool,
	};
};
