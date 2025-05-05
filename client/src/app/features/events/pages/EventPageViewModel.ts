import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router';
import { Event, TicketPool, TicketPools } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';
import { OrderService } from '../../../core/services/OrderService';
import { Order } from '../../../core/contracts/Order';

const eventService = EventService.getInstance();
const orderService = OrderService.getInstance();

export const useEventPageViewModel = () => {
	const { eventId } = useParams<{ eventId: string }>();
	const [event, setEvent] = useState<Event | null>(null);
	const [loading, setLoading] = useState<boolean>(true);
	const [error, setError] = useState<string | null>(null);
	const [isAvailable, setIsAvailable] = useState<boolean>(false);
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const [info, setInfo] = useState<string | null>(null);
	const [showInfoModal, setShowInfoModal] = useState(false);
	const navigate = useNavigate();

	const dateToLocaleString = (date: string | undefined): string => {
		if (!date) return '';
		return new Date(date).toLocaleDateString();
	};

	const getAddress = (
		street: string | undefined,
		building: string | undefined,
		room: string | undefined,
		code: string | undefined,
		post: string | undefined
	): string => {
		let address = street || '';
		if (building) address += `, ${building}`;
		if (room) address += `, ${room}`;
		if (code) address += `, ${code.substring(0, 3)}-${code.substring(3)}`;
		if (post) address += `, ${post}`;
		return address;
	};

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

	const handleOnAddToOrderByUser = async (accessToken: string) => {
		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		if (!event || !event.ticketPools || event.ticketPools.items.length === 0) {
			setError('No available tickets.');
			return;
		}

		const ticketPool = event.ticketPools.items.reduce((prev, current) => {
			return prev.priceAmount < current.priceAmount ? prev : current;
		});

		try {
			let order: Order | null = null;

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

	const handleOnAddToOrderWithoutUser = async () => {
		if (!eventId) {
			setError('Event ID is not provided.');
			return;
		}

		if (!event || !event.ticketPools || event.ticketPools.items.length === 0) {
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

	const handleOnAddToOrder = async () => {
		const accessToken = localStorage.getItem('authToken');
		if (!accessToken) {
			await handleOnAddToOrderWithoutUser();
			return;
		}

		await handleOnAddToOrderByUser(accessToken);
	};

	const handleCloseInfo = () => {
		setShowInfoModal(false);
		navigate(0);
	};

	const loadEvent = async () => {
		setLoading(true);
		setError(null);

		try {
			if (!eventId) {
				throw new Error('Event ID is not provided.');
			}

			const data = await eventService.getEvent(eventId);

			if (!data) {
				throw new Error('No data received from the server.');
			}

			setEvent(data);
			const available = checkTickets(data.ticketPools);
			setIsAvailable(available);
		} catch (err) {
			setError('Failed to load event.');
		} finally {
			setLoading(false);
		}
	};

	const checkTickets = (ticketPools: TicketPools): boolean => {
		return ticketPools.items.some((ticketPool: TicketPool) => {
			const now = new Date();
			const startDate = new Date(ticketPool.startDate);
			const endDate = new Date(ticketPool.endDate);
			return ticketPool.availableTickets > 0 && now >= startDate && now <= endDate;
		});
	};
	useEffect(() => {
		loadEvent();

		window.scrollTo(0, 0);
	}, [eventId]);

	return {
		event,
		loading,
		error,
		navigate,
		dateToLocaleString,
		getAddress,
		getPrice,
		showSuccessModal,
		setShowSuccessModal,
		isAvailable,
		handleOnAddToOrder,
		info,
		showInfoModal,
		handleCloseInfo,
	};
};
