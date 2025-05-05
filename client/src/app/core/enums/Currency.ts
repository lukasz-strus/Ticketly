export class Currency {
	static USD = {
		id: 1,
		name: 'USD',
	};
	static EUR = {
		id: 2,
		name: 'EUR',
	};
	static PLN = {
		id: 3,
		name: 'PLN',
	};

	static fromName(name: string): any {
		switch (name) {
			case 'USD':
				return Currency.USD;
			case 'EUR':
				return Currency.EUR;
			case 'PLN':
				return Currency.PLN;
			default:
				throw new Error(`Unknown currency: ${name}`);
		}
	}

	static fromId(id: number): any {
		switch (id) {
			case Currency.USD.id:
				return Currency.USD;
			case Currency.EUR.id:
				return Currency.EUR;
			case Currency.PLN.id:
				return Currency.PLN;
			default:
				throw new Error(`Unknown currency id: ${id}`);
		}
	}
}
