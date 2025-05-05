import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { useAuth } from '../../../core/context/AuthContext';
import { Order, OrderItem } from '../../../core/contracts/Order';
import { Currency } from '../../../core/enums/Currency';
import { OrderService } from '../../../core/services/OrderService';
import { EventService } from '../../../core/services/EventService';
import { UserService } from '../../../core/services/UserService';

const orderService = OrderService.getInstance();
const eventService = EventService.getInstance();
const userService = UserService.getInstance();

export const useOrderSummaryPageViewModel = () => {
	const [order, setOrder] = useState<Order | null>(null);

	const [firstName, setFirstName] = useState('');
	const [firstNameError, setFirstNameError] = useState<string | null>(null);

	const [lastName, setLastName] = useState('');
	const [lastNameError, setLastNameError] = useState<string | null>(null);

	const [street, setStreet] = useState('');
	const [streetError, setStreetError] = useState<string | null>(null);

	const [building, setBuilding] = useState('');
	const [buildingError, setBuildingError] = useState<string | null>(null);

	const [room, setRoom] = useState('');
	const [roomError, setRoomError] = useState<string | null>(null);

	const [code, setCode] = useState('');
	const [codeError, setCodeError] = useState<string | null>(null);

	const [post, setPost] = useState('');
	const [postError, setPostError] = useState<string | null>(null);

	const [error, setError] = useState<string | null>(null);
	const [loading, setLoading] = useState<boolean>(false);
	const [info, setInfo] = useState<string | null>(null);
	const [showInfoModal, setShowInfoModal] = useState(false);

	const { isLoggedIn } = useAuth();
	const navigator = useNavigate();

	const validateForm = () => {
		let hasError = false;

		setError(null);
		setFirstNameError(null);
		setLastNameError(null);
		setStreetError(null);
		setBuildingError(null);
		setRoomError(null);
		setCodeError(null);
		setPostError(null);

		if (!firstName) {
			setFirstNameError('Imię nie może być puste.');
			hasError = true;
		}
		if (!lastName) {
			setLastNameError('Nazwisko nie może być puste.');
			hasError = true;
		}

		if (!street) {
			setStreetError('Ulica nie może być pusta.');
			hasError = true;
		}
		if (!building) {
			setBuildingError('Budynek nie może być pusty.');
			hasError = true;
		}
		if (!code || code.length !== 6) {
			setCodeError('Kod pocztowy musi być w formacie 00-000.');
			hasError = true;
		}
		if (!post) {
			setPostError('Miasto nie może być puste.');
			hasError = true;
		}

		return !hasError;
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

	const completeOrder = async () => {
		if (!validateForm()) return;

		let orderToComplete = order;

		const token = localStorage.getItem('authToken');
		if (!token) {
			await orderService
				.createOrderWithoutUser(firstName, lastName, street, building, room, code.replace('-', ''), post)
				.then(async orderId => {
					if (!orderId) {
						console.log('Order ID not found after creating order.');
						return;
					}

					await orderService
						.getOrderById(orderId)
						.then(async result => {
							if (!result) {
								console.log('Order not found after creating order item.');
								return;
							}
							orderToComplete = result;
						})
						.catch(err => {
							console.log('Error fetching order:', err);
							return;
						});

					const eventId = localStorage.getItem('eventId');
					const ticketPoolId = localStorage.getItem('ticketPoolId');
					const quantity = localStorage.getItem('quantity');
					if (!eventId) {
						console.log('Event ID not found in local storage.');
						return;
					}
					if (!ticketPoolId) {
						console.log('Ticket Pool ID not found in local storage.');
						return;
					}
					if (!quantity) {
						console.log('Quantity not found in local storage.');
						return;
					}

					await orderService
						.createOrderItem(token, orderId, eventId, ticketPoolId, Number(quantity))
						.then(() => {})
						.catch(err => {
							console.log('Error creating order item:', err);
							return;
						});
				})
				.catch(err => {
					console.log('Error creating order item:', err);
					return;
				});

			if (!orderToComplete) {
				console.log('Order not found after creating order item.');
				return;
			}
		}

		if (!orderToComplete) {
			console.log('Order not found after creating order item.');
			return;
		}

		if (orderToComplete.id === '1') {
			setError('Dodaj przynajmniej jeden bilet do zamówienia.');
			return;
		}

		await orderService
			.completeOrder(token, orderToComplete?.id)
			.then(() => {
				setOrder(null);
			})
			.catch(err => {
				setInfo(err.response.data.detail);
				setShowInfoModal(true);
				return;
			});

		setInfo('Zamówienie zostało złożone. Sprawdź swoją skrzynkę e-mail.');
		localStorage.removeItem('eventId');
		localStorage.removeItem('ticketPoolId');
		localStorage.removeItem('quantity');
		setShowInfoModal(true);
	};

	const handleSuccessClose = () => {
		setShowInfoModal(false);
		navigator('/');
	};

	const setAndCheckCode = (input: string) => {
		let formattedInput = input.replace(/[^0-9-]/g, '');

		if (formattedInput.length > 2 && !formattedInput.includes('-')) {
			formattedInput = `${formattedInput.slice(0, 2)}-${formattedInput.slice(2)}`;
		}

		if (formattedInput.length > 6) {
			formattedInput = formattedInput.slice(0, 6);
		}

		setCodeError(formattedInput.length < 6 ? 'Kod pocztowy musi być w formacie 00-000.' : null);
		setCode(formattedInput);
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

	const loadUser = async (token: string) => {
		setLoading(true);

		try {
			if (!token) {
				await createTmpOrder();
				return;
			}
			const data = await userService.getMeProfile(token);
			if (!data) {
				setError('');
				return;
			}
			setFirstName(data.firstName);
			setLastName(data.lastName);
			setStreet(data.addressStreet);
			setBuilding(data.addressBuilding);
			setRoom(data.addressRoom);
			setAndCheckCode(data.addressCode);
			setPost(data.addressPost);
		} catch (err) {
			setError('');
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		loadOrder();
		const token = localStorage.getItem('authToken');
		if (token) {
			loadUser(token);
		}
		window.scrollTo(0, 0);
	}, []);

	return {
		firstName,
		setFirstName,
		firstNameError,
		lastName,
		setLastName,
		lastNameError,
		street,
		setStreet,
		streetError,
		building,
		setBuilding,
		buildingError,
		room,
		setRoom,
		roomError,
		code,
		setCode,
		codeError,
		post,
		setPost,
		postError,
		error,
		loading,
		handleSuccessClose,
		setAndCheckCode,
		isLoggedIn,
		order,
		getOrderPrice,
		completeOrder,
		info,
		showInfoModal,
	};
};
