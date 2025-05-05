import { ApiService } from './ApiService.ts';
import ApiEndpoints from '../constants/ApiEndpoints.ts';

export class AuthService extends ApiService {
	private static instance: AuthService | null = null;

	private constructor() {
		super(ApiEndpoints.BASE_URL);
	}

	static getInstance(): AuthService {
		if (!AuthService.instance) {
			AuthService.instance = new AuthService();
		}
		return AuthService.instance;
	}

	async loginUser(email: string, password: string): Promise<string> {
		const { data } = await this.API.post(ApiEndpoints.AUTH.LOGIN, { email, password });
		return data.token;
	}

	async registerUser(
		email: string,
		password: string,
		firstName: string,
		lastName: string,
		street: string,
		building: string,
		room: string | null,
		code: string,
		post: string
	): Promise<void> {
		await this.API.post(ApiEndpoints.AUTH.REGISTER, {
			email,
			password,
			firstName,
			lastName,
			street,
			building,
			room,
			code,
			post,
		});
	}
}
