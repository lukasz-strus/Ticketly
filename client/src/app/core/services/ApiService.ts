import axios, { AxiosInstance } from 'axios';

export interface FetchParams {
	filters?: string;
	sorts?: string;
	page?: number;
	pageSize?: number;
}

export abstract class ApiService {
	protected API: AxiosInstance;

	constructor(baseURL: string) {
		this.API = axios.create({
			baseURL,
		});
	}

	protected setAuthorizationHeader(token: string) {
		return {
			headers: {
				Authorization: `Bearer ${token}`,
			},
		};
	}

	protected buildQueryParams(params: FetchParams): Record<string, string | number> {
		const queryParams: Record<string, string | number> = {};

		if (params.filters) {
			queryParams.Filters = params.filters;
		}
		if (params.sorts) {
			queryParams.Sorts = params.sorts;
		}
		if (params.page !== undefined) {
			queryParams.Page = params.page;
		}
		if (params.pageSize !== undefined) {
			queryParams.PageSize = params.pageSize;
		}

		return queryParams;
	}
}
