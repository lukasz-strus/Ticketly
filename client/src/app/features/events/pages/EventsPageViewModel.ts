import { useEffect, useState } from 'react';
import { Event, Events } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';
import { useNavigate, useParams } from 'react-router';
import { Order } from '../../../core/contracts/Order';
import { OrderService } from '../../../core/services/OrderService';

const eventService = EventService.getInstance();
const orderService = OrderService.getInstance();

export const useEventsPageViewModel = () => {
	const [events, setEvents] = useState<Event[]>([]);
	const [categoryName, setCategoryName] = useState<string | null>(null);
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const [info, setInfo] = useState<string | null>(null);
	const [showInfoModal, setShowInfoModal] = useState(false);
	const navigate = useNavigate();
	const { categoryId } = useParams<{ categoryId: string }>();

	const handleOnAddToOrderByUser = async (eventId: string, accessToken: string) => {
		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		let event: Event | null = null as Event | null;

		await eventService
			.getEvent(eventId)
			.then(res => {
				event = res;
			})
			.catch(err => {
				if (err.response?.status === 404) {
					console.log('Event not found.');
				} else {
					console.log('Failed to load event details.');
				}
			});

		if (!event) {
			setError('Event not found.');
			return;
		}

		if (!event.ticketPools || event.ticketPools.items.length === 0) {
			setError('No available tickets.');
			return;
		}

		const ticketPool = event.ticketPools.items.reduce((prev, current) => {
			return prev.priceAmount < current.priceAmount ? prev : current;
		});

		try {
			let order: Order | null = null as Order | null;

			try {
				order = await orderService.getPendingOrder(accessToken);
			} catch (err: any) {
				if (err.response?.status === 404) {
					const newOrderId = await orderService.createOrder(accessToken);
					if (!newOrderId) {
						setError('Failed to create order.');
						return;
					}

					order = await orderService.getPendingOrder(accessToken);
				} else {
					throw err;
				}
			}

			if (!order) {
				setError('Order not found.');
				return;
			}

			const existingItem = order.orderItems.items.find(
				item => item.eventId === eventId && item.ticketPoolId === ticketPool.id
			);

			if (existingItem) {
				await orderService.updateOrderItem(accessToken, order.id, existingItem.id, existingItem.quantity + 1);
			} else {
				await orderService.createOrderItem(accessToken, order.id, eventId, ticketPool.id, 1);
			}

			setShowSuccessModal(true);
			navigate('/order');
		} catch (err: any) {
			setInfo(err.response?.data?.detail || 'Unexpected error occurred.');
			setShowInfoModal(true);
		}
	};

	const handleOnAddToOrderWithoutUser = async (eventId: string) => {
		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		let event: Event | null = null as Event | null;

		await eventService
			.getEvent(eventId)
			.then(res => {
				event = res;
			})
			.catch(err => {
				if (err.response?.status === 404) {
					console.log('Event not found.');
				} else {
					console.log('Failed to load event details.');
				}
			});

		if (!event) {
			setError('Event not found.');
			return;
		}

		if (!event.ticketPools || event.ticketPools.items.length === 0) {
			setError('No available tickets.');
			return;
		}

		const ticketPool = event.ticketPools.items.reduce((prev, current) => {
			return prev.priceAmount < current.priceAmount ? prev : current;
		});

		localStorage.setItem('eventId', eventId);
		localStorage.setItem('ticketPoolId', ticketPool.id);
		localStorage.setItem('quantity', '1');
		navigate('/order');
	};

	const handleOnAddToOrder = async (eventId: string) => {
		const accessToken = localStorage.getItem('authToken');
		if (!accessToken) {
			await handleOnAddToOrderWithoutUser(eventId);
			return;
		}

		await handleOnAddToOrderByUser(eventId, accessToken);
	};

	const loadEvents = async () => {
		setLoading(true);
		setError(null);

		try {
			let events: Events;
			let categoryName: string | null = null;

			if (categoryId) {
				events = await eventService.getEventsByCategoryId(categoryId);
				const category = await eventService.getCategory(categoryId);
				if (category) {
					categoryName = category.name;
				}
			} else {
				events = await eventService.getEvents();
			}

			if (!events) {
				throw new Error('No data received from the server.');
			}

			setEvents(events.items);
			setCategoryName(categoryName);
		} catch (err) {
			setError('Failed to load events.');
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		loadEvents();
	}, [categoryId]);

	return {
		events,
		loading,
		error,
		navigate,
		showSuccessModal,
		setShowSuccessModal,
		handleOnAddToOrder,
		info,
		showInfoModal,
		categoryId,
		categoryName,
	};
};
