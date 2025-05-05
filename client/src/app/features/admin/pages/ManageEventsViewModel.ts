import { useEffect, useState } from 'react';
import { Category, Event } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';
import { useNavigate } from 'react-router';

const eventService = EventService.getInstance();

export const useManageEventsViewModel = () => {
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [categories, setCategories] = useState<Category[]>([]);
	const [events, setEvents] = useState<Event[]>([]);
	const [selectedEventId, setSelectedEventId] = useState<string | null>(null);

	const [showEventFormModal, setShowEventFormModal] = useState(false);
	const [showTicketPoolsModal, setShowTicketPoolsModal] = useState(false);

	const navigate = useNavigate();

	const handleOnCreateEvent = () => {
		setShowEventFormModal(true);
	};

	const handleCloseEventForm = () => {
		setShowTicketPoolsModal(false);
		navigate(0);
	};

	const handleCloseTicketPoolsModal = () => {
		setShowEventFormModal(false);
		navigate(0);
	};

	const dateToLocaleString = (date: string | undefined): string => {
		if (!date) return '';
		return new Date(date).toLocaleDateString();
	};

	const handleOnEditEvent = (eventId: string) => {
		if (!eventId) {
			console.log('Event ID is not provided.');
			return;
		}

		setSelectedEventId(eventId);
		setShowEventFormModal(true);
	};

	const handleTicketPoolsEvent = (eventId: string) => {
		if (!eventId) {
			console.log('Event ID is not provided.');
			return;
		}

		setSelectedEventId(eventId);
		setShowTicketPoolsModal(true);
	};

	const handleOnDeleteEvent = async (eventId: string) => {
		if (!eventId) {
			console.log('Event ID is not provided.');
			return;
		}

		const token = localStorage.getItem('authToken');
		if (!token) {
			console.log('User is not authenticated.');
			return;
		}

		setLoading(true);
		setError(null);

		try {
			await eventService.deleteEvent(token, eventId);
			setEvents(prevEvents => prevEvents.filter(event => event.id !== eventId));
		} catch (err) {
			if (err instanceof Error) {
				console.log(err.message);
			} else {
				console.log('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	const fetchCategories = async () => {
		setLoading(true);
		setError(null);
		try {
			const response = await eventService.getCategories();
			setCategories(response.items);
		} catch (err) {
			if (err instanceof Error) {
				console.log(err.message);
			} else {
				console.log('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	const fetchEvents = async () => {
		setLoading(true);
		setError(null);

		try {
			const response = await eventService.getEvents();
			setEvents(response.items);
		} catch (err) {
			if (err instanceof Error) {
				console.log(err.message);
			} else {
				console.log('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		fetchCategories();
		fetchEvents();
	}, []);

	return {
		loading,
		error,
		categories,
		events,
		dateToLocaleString,
		handleOnDeleteEvent,
		handleOnCreateEvent,
		showEventFormModal,
		handleCloseEventForm,
		selectedEventId,
		handleOnEditEvent,
		showTicketPoolsModal,
		handleCloseTicketPoolsModal,
		handleTicketPoolsEvent,
	};
};
