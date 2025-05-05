import { ApiService } from './ApiService.ts';
import ApiEndpoints from '../constants/ApiEndpoints.ts';
import { User } from '../contracts/User.ts';
import { Order } from '../contracts/Order.ts';

export class UserService extends ApiService {
	private static instance: UserService | null = null;

	private constructor() {
		super(ApiEndpoints.BASE_URL);
	}

	static getInstance(): UserService {
		if (!UserService.instance) {
			UserService.instance = new UserService();
		}

		return UserService.instance;
	}

	async getMeProfile(token: string): Promise<User> {
		const { data } = await this.API.get(ApiEndpoints.USER.ME, this.setAuthorizationHeader(token));
		return data;
	}

	async getOrders(token: string): Promise<{ items: Order[] }> {
		const { data } = await this.API.get(ApiEndpoints.USER.ORDERS, this.setAuthorizationHeader(token));
		return data;
	}
}
