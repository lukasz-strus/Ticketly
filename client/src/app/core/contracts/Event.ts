export interface Category {
	id: string;
	name: string;
}

export interface Categories {
	items: Category[];
}

export interface TicketPool {
	id: string;
	availableTickets: number;
	priceAmount: number;
	priceCurrencyId: number;
	price: string;
	startDate: string;
	endDate: string;
}

export interface TicketPoolRequest {
	availableTickets: number;
	price: number;
	currencyId: number;
	startDate: string;
	endDate: string;
}

export interface TicketPools {
	items: TicketPool[];
}

export interface Event {
	id: string;
	name: string;
	categoryId: string;
	categoryName: string;
	description: string;
	locationStreet: string;
	locationBuilding: string;
	locationRoom: string;
	locationCode: string;
	locationPost: string;
	date: string;
	imageUrl: string;
	ticketPools: TicketPools;
}

export interface Events {
	items: Event[];
}
