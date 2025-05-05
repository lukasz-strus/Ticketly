import { useEffect, useState } from 'react';
import { useAuth } from '../../core/context/AuthContext';
import { useLocation, useNavigate } from 'react-router';
import { OrderService } from '../../core/services/OrderService';
import { UserService } from '../../core/services/UserService';

const orderService = OrderService.getInstance();
const userService = UserService.getInstance();

export const useUserMenuViewModel = () => {
	const [showLoginModal, setShowLoginModal] = useState(false);
	const { isLoggedIn, setIsLoggedIn, userName, setUserName, userRole, setUserRole } = useAuth();
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const location = useLocation();
	const navigate = useNavigate();

	function getNavLinkClass(path: string): string {
		return location.pathname === path ? 'fw-bold' : '';
	}

	function handleLogout() {
		localStorage.removeItem('authToken');
		setIsLoggedIn(false);
		setUserName(null);
		setUserRole(null);

		setShowSuccessModal(true);
	}

	function handleSuccessClose() {
		setShowSuccessModal(false);
		navigate(0);
	}

	const handleLogin = async () => {
		setShowLoginModal(false);

		const token = localStorage.getItem('authToken');
		if (token === null) {
			return;
		}
		await userService
			.getMeProfile(token)
			.then(response => {
				if (response) {
					const user = response;
					if (!user) {
						localStorage.removeItem('authToken');
						return;
					}
					setIsLoggedIn(true);
					const userName = user.firstName + ' ' + user.lastName;
					setUserName(userName);
					setUserRole(user.roleName);
				} else {
					localStorage.removeItem('authToken');
				}
			})
			.catch(error => {
				console.error('Error fetching user profile:', error);
				localStorage.removeItem('authToken');
			});

		await orderService
			.getPendingOrder(token)
			.then(() => {
				localStorage.removeItem('eventId');
				localStorage.removeItem('ticketPoolId');
				localStorage.removeItem('quantity');
			})
			.catch(async () => {
				const eventId = localStorage.getItem('eventId');
				const ticketPoolId = localStorage.getItem('ticketPoolId');
				const quantity = localStorage.getItem('quantity');

				if (!eventId || !ticketPoolId || !quantity) {
					return;
				}
				const orderId = await orderService.createOrder(token);

				if (!orderId) {
					return;
				}

				await orderService.createOrderItem(token, orderId, eventId, ticketPoolId, Number(quantity));
				localStorage.removeItem('eventId');
				localStorage.removeItem('ticketPoolId');
				localStorage.removeItem('quantity');
			});
	};

	useEffect(() => {
		handleLogin();
	}, []);

	return {
		showLoginModal,
		setShowLoginModal,
		isLoggedIn,
		userName,
		getNavLinkClass,
		handleLogout,
		handleSuccessClose,
		showSuccessModal,
		handleLogin,
		userRole,
	};
};
