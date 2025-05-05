import { useEffect, useState } from 'react';
import { UserService } from '../../../core/services/UserService';
import { Order } from '../../../core/contracts/Order';

const userService = UserService.getInstance();

export const useOrderHistoryPageViewModel = () => {
	const [orders, setOrders] = useState<Order[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);

	function dateToLocaleString(date: string): string {
		return new Date(date).toLocaleDateString() + 'r.';
	}

	const fetchOrders = async () => {
		setLoading(true);
		setError(null);
		try {
			const token = localStorage.getItem('authToken');
			if (!token) {
				throw new Error('User is not authenticated.');
			}
			const response = await userService.getOrders(token);
			setOrders(response.items.filter(order => order.statusId == 2));
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
		fetchOrders();
	}, []);

	return {
		orders,
		loading,
		error,
		dateToLocaleString,
	};
};
