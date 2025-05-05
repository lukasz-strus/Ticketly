import { ApiService } from './ApiService.ts';
import ApiEndpoints from '../constants/ApiEndpoints.ts';
import { Order } from '../contracts/Order.ts';

export class OrderService extends ApiService {
	private static instance: OrderService | null = null;

	private constructor() {
		super(ApiEndpoints.BASE_URL);
	}

	static getInstance(): OrderService {
		if (!OrderService.instance) {
			OrderService.instance = new OrderService();
		}

		return OrderService.instance;
	}

	async getPendingOrder(token: string): Promise<Order> {
		const { data } = await this.API.get(ApiEndpoints.ORDER.PENDING, this.setAuthorizationHeader(token));
		return data;
	}

	async getOrderById(id: string): Promise<Order> {
		const { data } = await this.API.get(ApiEndpoints.ORDER.GET, {
			params: { id },
		});
		return data;
	}

	async createOrder(token: string | null): Promise<string> {
		const { data } = await this.API.post(
			ApiEndpoints.ORDER.CREATE,
			{},
			token ? this.setAuthorizationHeader(token) : undefined
		);
		return data.id;
	}

	async createOrderWithoutUser(
		firstName: string,
		lastName: string,
		addressStreet: string,
		addressBuilding: string,
		addressRoom: string | null,
		addressCode: string,
		addressPost: string
	): Promise<string> {
		const { data } = await this.API.post(ApiEndpoints.ORDER.CREATE, {
			firstName,
			lastName,
			addressStreet,
			addressBuilding,
			addressRoom,
			addressCode,
			addressPost,
		});
		return data.id;
	}

	async createOrderItem(
		token: string | null,
		orderId: string,
		eventId: string,
		ticketPoolId: string,
		quantity: number
	) {
		await this.API.post(
			ApiEndpoints.ORDER.CREATE_ITEM(orderId),
			{
				eventId,
				ticketPoolId,
				quantity,
			},
			token ? this.setAuthorizationHeader(token) : undefined
		);
	}

	async deleteOrderItem(token: string | null, orderId: string, orderItemId: string) {
		await this.API.delete(
			ApiEndpoints.ORDER.DELETE_ITEM(orderId, orderItemId),
			token ? this.setAuthorizationHeader(token) : undefined
		);
	}

	async updateOrderItem(token: string | null, orderId: string, orderItemId: string, quantity: number) {
		await this.API.put(
			ApiEndpoints.ORDER.UPDATE_ITEM(orderId, orderItemId),
			{
				quantity,
			},
			token ? this.setAuthorizationHeader(token) : undefined
		);
	}

	async completeOrder(token: string | null, orderId: string) {
		await this.API.post(
			ApiEndpoints.ORDER.COMPLETE(orderId),
			{},
			token ? this.setAuthorizationHeader(token) : undefined
		);
	}

	async cancelOrder(token: string | null, orderId: string) {
		await this.API.post(ApiEndpoints.ORDER.CANCEL(orderId), token ? this.setAuthorizationHeader(token) : undefined);
	}

	async openOrder(token: string | null, orderId: string) {
		await this.API.post(ApiEndpoints.ORDER.OPEN(orderId), {}, token ? this.setAuthorizationHeader(token) : undefined);
	}
}
