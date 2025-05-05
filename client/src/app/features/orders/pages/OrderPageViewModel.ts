import { useEffect, useState } from 'react';
import { Order, OrderItem } from '../../../core/contracts/Order';
import { OrderService } from '../../../core/services/OrderService';
import { Currency } from '../../../core/enums/Currency';
import { useNavigate } from 'react-router';
import { EventService } from '../../../core/services/EventService';
import { useAuth } from '../../../core/context/AuthContext';

const orderService = OrderService.getInstance();
const eventService = EventService.getInstance();

export const useOrderPageViewModel = () => {
	const [order, setOrder] = useState<Order | null>(null);
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [info, setInfo] = useState<string | null>(null);
	const [showInfoModal, setShowInfoModal] = useState(false);
	const [confirmationQuestion, setConfirmationQuestion] = useState<string | null>(null);
	const [showConfirmationModal, setShowConfirmationModal] = useState(false);
	const { isLoggedIn } = useAuth();
	const navigate = useNavigate();

	const checkNewQuantity = async (orderItemId: string, newQuantity: number): Promise<boolean> => {
		if (newQuantity < 1) {
			setConfirmationQuestion(`Czy na pewno chcesz usunąć ten bilet?`);
			setShowConfirmationModal(true);
			return false;
		}

		let eventId: string | null = null;
		const token = localStorage.getItem('authToken');
		if (token) {
			await orderService
				.getPendingOrder(token)
				.then(data => {
					if (!data) {
						setError('');
						return false;
					}
					const orderItem = data.orderItems.items.find(item => item.id === orderItemId);
					if (!orderItem) {
						setError('');
						return false;
					}
					eventId = orderItem.eventId;
				})
				.catch(() => {
					setError('');
					return false;
				});
		} else {
			eventId = localStorage.getItem('eventId');
		}
		if (!eventId) {
			setError('');
			return false;
		}

		let availableTickets = 0;
		await eventService
			.getEvent(eventId)
			.then(event => {
				if (!event) {
					setError('');
					return false;
				}
				const ticketPool = event.ticketPools.items.find(
					ticketPool => ticketPool.id === order?.orderItems.items[0].ticketPoolId
				);
				if (!ticketPool) {
					setError('');
					return false;
				}
				availableTickets = ticketPool.availableTickets;
			})
			.catch(() => {
				setError('');
				return false;
			});

		if (newQuantity > availableTickets) {
			setInfo(`Tylko ${availableTickets} biletów dostępnych na to wydarzenie.`);
			setShowInfoModal(true);
			return false;
		}

		return true;
	};

	const changeQuantity = async (orderId: string, orderItemId: string, changeQuantity: number) => {
		const quantity = order?.orderItems.items.find(item => item.id === orderItemId)?.quantity || 0;
		const newQuantity = quantity + changeQuantity;

		var isCorrect = await checkNewQuantity(orderItemId, newQuantity);
		if (!isCorrect) {
			return;
		}

		const token = localStorage.getItem('authToken');

		if (!token) {
			const orderItem = order?.orderItems.items.find(item => item.id === orderItemId);
			if (orderItem) {
				orderItem.quantity = newQuantity;
				localStorage.setItem('quantity', newQuantity.toString());
				order?.orderItems.items.splice(order?.orderItems.items.indexOf(orderItem), 1, orderItem);
				setOrder(order);
				navigate(0);
			}
			return;
		}

		await orderService
			.updateOrderItem(token, orderId, orderItemId, newQuantity)
			.then(() => {
				loadOrder();
			})
			.catch(err => {
				setInfo(err.response.data.detail);
				setShowInfoModal(true);
			});
	};

	const removeOrderItem = async (orderId: string, orderItemId: string) => {
		const token = localStorage.getItem('authToken');
		if (!token) {
			localStorage.removeItem('eventId');
			localStorage.removeItem('ticketPoolId');
			localStorage.removeItem('quantity');
			navigate(0);
			return;
		}

		const isLastItem = order?.orderItems.items.length === 1;

		if (isLastItem) {
			await orderService
				.cancelOrder(token, orderId)
				.then(() => {
					setOrder(null);
					navigate('/');
				})
				.catch(err => {
					setInfo(err.response.data.detail);
					setShowInfoModal(true);
				});
		}

		await orderService
			.deleteOrderItem(token, orderId, orderItemId)
			.then(() => {
				loadOrder();
			})
			.catch(err => {
				setInfo(err.response.data.detail);
				setShowInfoModal(true);
			});
	};

	const handleConfirmationClose = () => {
		setShowConfirmationModal(false);
		setConfirmationQuestion(null);
	};

	const handleConfirmationAccept = async () => {
		if (order && order.orderItems.items.length > 0) {
			await removeOrderItem(order.id, order.orderItems.items[0].id);
		}
		setShowConfirmationModal(false);
		setConfirmationQuestion(null);
	};

	const handleCloseInfo = () => {
		setShowInfoModal(false);
		navigate(0);
	};

	const getOrderPrice = (): string => {
		if (!order || !order.orderItems) {
			return '0.00';
		}
		const orderItemsByCurrency = groupByCurrencyId(order.orderItems.items);
		return Object.entries(orderItemsByCurrency)
			.map(([currencyId, items]) => {
				const totalPrice = items.reduce((acc, item) => acc + item.priceAmount * item.quantity, 0);
				const currency = Currency.fromId(Number(currencyId)).name;
				return `${totalPrice} ${currency}`;
			})
			.join(' | ');
	};

	const groupByCurrencyId = (items: OrderItem[]): Record<number, OrderItem[]> => {
		return items.reduce((acc, item) => {
			if (!acc[item.priceCurrencyId]) {
				acc[item.priceCurrencyId] = [];
			}
			acc[item.priceCurrencyId].push(item);
			return acc;
		}, {} as Record<number, OrderItem[]>);
	};

	const completeOrder = () => {
		navigate('/order/summary');
	};

	const createTmpOrder = async () => {
		const eventId = localStorage.getItem('eventId');
		const ticketPoolId = localStorage.getItem('ticketPoolId');
		const quantity = localStorage.getItem('quantity');

		if (!eventId) {
			setError('');
			return;
		}
		if (!ticketPoolId) {
			setError('');
			return;
		}
		if (!quantity) {
			setError('');
			return;
		}

		const event = await eventService.getEvent(eventId);
		if (!event) {
			setError('');
			return;
		}
		const ticketPool = event.ticketPools.items.find(ticketPool => ticketPool.id === ticketPoolId);
		if (!ticketPool) {
			setError('');
			return;
		}

		const order: Order = {
			id: '1',
			firstName: '',
			lastName: '',
			addressStreet: '',
			addressBuilding: '',
			addressRoom: '',
			addressCode: '',
			addressPost: '',
			createdAt: '',
			statusId: 0,
			status: '',
			orderItems: {
				items: [
					{
						id: '1',
						orderId: '1',
						ticketPoolId: ticketPool.id,
						eventId: event.id,
						eventImgUrl: event.imageUrl,
						eventName: event.name,
						quantity: Number(quantity),
						priceAmount: ticketPool.priceAmount,
						priceCurrencyId: ticketPool.priceCurrencyId,
						price: `${ticketPool.priceAmount} ${Currency.fromId(ticketPool.priceCurrencyId).name}`,
					},
				],
			},
		};

		setOrder(order);
		setError(null);
		setLoading(false);
	};

	const loadOrder = async () => {
		setLoading(true);
		setError(null);

		try {
			const token = localStorage.getItem('authToken');
			if (!token) {
				await createTmpOrder();
				return;
			}
			const data = await orderService.getPendingOrder(token);
			if (!data) {
				setError('');
				return;
			}

			setOrder(data);
		} catch (err) {
			setError('');
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		loadOrder();
		window.scrollTo(0, 0);
	}, []);

	return {
		order,
		setOrder,
		loading,
		error,
		getOrderPrice,
		changeQuantity,
		info,
		handleCloseInfo,
		showInfoModal,
		confirmationQuestion,
		showConfirmationModal,
		handleConfirmationClose,
		handleConfirmationAccept,
		completeOrder,
		isLoggedIn,
	};
};
