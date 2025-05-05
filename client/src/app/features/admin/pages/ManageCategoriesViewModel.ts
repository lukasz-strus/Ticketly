import { useEffect, useState } from 'react';
import { Category } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';

const eventService = EventService.getInstance();

export const useManageCategoriesViewModel = () => {
	const [categories, setCategories] = useState<Category[]>([]);
	const [loading, setLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [newCategoryName, setNewCategoryName] = useState<string>('');

	const fetchCategories = async () => {
		setLoading(true);
		setError(null);
		try {
			const response = await eventService.getCategories();
			setCategories(response.items);
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

	const handleOnDeleteCategory = async (categoryId: string) => {
		if (!categoryId) {
			console.log('Category ID is not provided.');
			return;
		}

		const token = localStorage.getItem('authToken');
		if (!token) {
			console.log('User is not authenticated.');
			return;
		}

		setLoading(true);
		setError(null);

		try {
			await eventService.deleteCategory(token, categoryId);
			setCategories(prevCategories => prevCategories.filter(category => category.id !== categoryId));
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

	const handleOnCreateCategory = async () => {
		if (!newCategoryName) {
			console.log('Category is not provided.');
			return;
		}

		const token = localStorage.getItem('authToken');
		if (!token) {
			console.log('User is not authenticated.');
			return;
		}

		setLoading(true);
		setError(null);

		try {
			const newCategory = await eventService.createCategory(token, newCategoryName);
			if (!newCategory) {
				console.log('Failed to create category.');
				return;
			}

			await fetchCategories();
		} catch (err) {
			if (err instanceof Error) {
				console.log(err.message);
			} else {
				console.log('An unexpected error occurred.');
			}
		} finally {
			setLoading(false);
			setNewCategoryName('');
		}
	};

	useEffect(() => {
		const token = localStorage.getItem('authToken');
		if (!token) {
			console.log('User is not authenticated.');
			setError('Użytkownik nie jest uwierzytelniony.');
			return;
		}
		fetchCategories();
	}, []);

	return {
		categories,
		loading,
		error,
		handleOnDeleteCategory,
		handleOnCreateCategory,
		newCategoryName,
		setNewCategoryName,
	};
};
