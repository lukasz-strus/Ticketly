class ApiEndpoints {
	static readonly BASE_URL = 'https://localhost:7080/api';

	static readonly AUTH = {
		LOGIN: '/auth/login',
		REGISTER: '/auth/register',
	};

	static readonly USER = {
		ME: '/users/me',
		ORDERS: '/users/me/orders',
	};

	static readonly ORDER = {
		GET: `/orders`,
		PENDING: '/orders/pending',
		CREATE: '/orders',
		COMPLETE: (orderId: string) => `/orders/complete/${orderId}`,
		CANCEL: (orderId: string) => `/orders/cancel/${orderId}`,
		OPEN: (orderId: string) => `/orders/open/${orderId}`,
		CREATE_ITEM: (eventId: string) => `/orders/${eventId}/order-item`,
		DELETE_ITEM: (orderId: string, orderItemId: string) => `/orders/${orderId}/order-item/${orderItemId}`,
		UPDATE_ITEM: (orderId: string, orderItemId: string) => `/orders/${orderId}/order-item/${orderItemId}`,
	};

	static readonly EVENTS = {
		GET: '/events',
		CREATE: '/events',
		UPDATE: (eventId: string) => `/events/${eventId}`,
		DELETE: (eventId: string) => `/events/${eventId}`,
		GET_BY_ID: (eventId: string) => `/events/${eventId}`,
		GET_BY_CATEGORY_ID: (categoryId: string) => `/categories/${categoryId}/events`,
		GET_CATEGORIES: '/categories',
		GET_CATEGORIES_BY_ID: (categoryId: string) => `/categories/${categoryId}`,
		CREATE_CATEGORY: '/categories',
		DELETE_CATEGORY: (categoryId: string) => `/categories/${categoryId}`,
		CREATE_TICKET_POOL: (eventId: string) => `/events/${eventId}/ticket-pool`,
		UPDATE_TICKET_POOL: (eventId: string, ticketPoolId: string) => `/events/${eventId}/ticket-pool/${ticketPoolId}`,
		DELETE_TICKET_POOL: (eventId: string, ticketPoolId: string) => `/events/${eventId}/ticket-pool/${ticketPoolId}`,
	};
}

export default ApiEndpoints;
