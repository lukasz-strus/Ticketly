import ApiEndpoints from '../constants/ApiEndpoints';
import { Categories, Events, Event, TicketPoolRequest, Category } from '../contracts/Event';
import { ApiService } from './ApiService';

export class EventService extends ApiService {
	private static instance: EventService | null = null;

	private constructor() {
		super(ApiEndpoints.BASE_URL);
	}

	static getInstance(): EventService {
		if (!EventService.instance) {
			EventService.instance = new EventService();
		}

		return EventService.instance;
	}

	async getEvents(): Promise<Events> {
		const { data } = await this.API.get(ApiEndpoints.EVENTS.GET);

		return data;
	}

	async createEvent(token: string, event: Event): Promise<string> {
		const request = {
			name: event.name,
			categoryId: event.categoryId,
			description: event.description,
			locationStreet: event.locationStreet,
			locationBuilding: event.locationBuilding,
			locationRoom: event.locationRoom,
			locationCode: event.locationCode,
			locationPost: event.locationPost,
			date: event.date,
			imageUrl: event.imageUrl,
		};
		const { data } = await this.API.post(ApiEndpoints.EVENTS.CREATE, request, this.setAuthorizationHeader(token));

		return data;
	}

	async updateEvent(token: string, eventId: string, event: Event): Promise<void> {
		await this.API.put(
			ApiEndpoints.EVENTS.UPDATE(eventId),
			{
				name: event.name,
				categoryId: event.categoryId,
				description: event.description,
				locationStreet: event.locationStreet,
				locationBuilding: event.locationBuilding,
				locationRoom: event.locationRoom,
				locationCode: event.locationCode,
				locationPost: event.locationPost,
				date: event.date,
				imageUrl: event.imageUrl,
			},
			this.setAuthorizationHeader(token)
		);
	}

	async deleteEvent(token: string, eventId: string): Promise<void> {
		await this.API.delete(ApiEndpoints.EVENTS.DELETE(eventId), this.setAuthorizationHeader(token));
	}

	async getEvent(eventId: string): Promise<Event> {
		const { data } = await this.API.get(ApiEndpoints.EVENTS.GET_BY_ID(eventId));

		return data;
	}

	async getCategories(): Promise<Categories> {
		const { data } = await this.API.get(ApiEndpoints.EVENTS.GET_CATEGORIES);

		return data;
	}

	async getCategory(categoryId: string): Promise<Category> {
		const { data } = await this.API.get(ApiEndpoints.EVENTS.GET_CATEGORIES_BY_ID(categoryId));

		return data;
	}

	async createCategory(token: string, name: string): Promise<string> {
		const { data } = await this.API.post(
			ApiEndpoints.EVENTS.CREATE_CATEGORY,
			{
				name,
			},
			this.setAuthorizationHeader(token)
		);

		return data;
	}

	async deleteCategory(token: string, categoryId: string): Promise<void> {
		await this.API.delete(ApiEndpoints.EVENTS.DELETE_CATEGORY(categoryId), this.setAuthorizationHeader(token));
	}

	async getEventsByCategoryId(categoryId: string): Promise<Events> {
		const { data } = await this.API.get(ApiEndpoints.EVENTS.GET_BY_CATEGORY_ID(categoryId));

		return data;
	}

	async createTicketPool(token: string, eventId: string, ticketPool: TicketPoolRequest): Promise<void> {
		await this.API.post(
			ApiEndpoints.EVENTS.CREATE_TICKET_POOL(eventId),
			ticketPool,
			this.setAuthorizationHeader(token)
		);
	}

	async updateTicketPool(
		token: string,
		eventId: string,
		ticketPoolId: string,
		ticketPool: TicketPoolRequest
	): Promise<void> {
		await this.API.put(
			ApiEndpoints.EVENTS.UPDATE_TICKET_POOL(eventId, ticketPoolId),
			ticketPool,
			this.setAuthorizationHeader(token)
		);
	}

	async deleteTicketPool(token: string, eventId: string, ticketPoolId: string): Promise<void> {
		await this.API.delete(
			ApiEndpoints.EVENTS.DELETE_TICKET_POOL(eventId, ticketPoolId),
			this.setAuthorizationHeader(token)
		);
	}
}
