export interface Order {
	id: string;
	firstName: string;
	lastName: string;
	addressStreet: string;
	addressBuilding: string;
	addressRoom: string;
	addressCode: string;
	addressPost: string;
	createdAt: string;
	statusId: number;
	status: string;
	orderItems: OrderItems;
}

export interface OrderItems {
	items: OrderItem[];
}

export interface OrderItem {
	id: string;
	orderId: string;
	ticketPoolId: string;
	eventId: string;
	eventImgUrl: string;
	eventName: string;
	quantity: number;
	priceAmount: number;
	priceCurrencyId: number;
	price: string;
}
