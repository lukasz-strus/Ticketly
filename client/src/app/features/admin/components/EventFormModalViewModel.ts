import { useEffect, useState } from 'react';
import { Category, Event } from '../../../core/contracts/Event';
import { EventService } from '../../../core/services/EventService';
import { useAuth } from '../../../core/context/AuthContext';

const eventService = EventService.getInstance();

export const useEventFormModalViewModel = (onClose: () => void, eventId: string | null) => {
	const [name, setName] = useState('');
	const [nameError, setNameError] = useState<string | null>(null);

	const [serverCategories, setServerCategories] = useState<Category[]>([]);
	const [categories, setCategories] = useState<string[]>([]);
	const [selectedCategory, setSelectedCategory] = useState<string>('');

	const [description, setDescription] = useState('');
	const [descriptionError, setDescriptionError] = useState<string | null>(null);

	const [street, setStreet] = useState('');
	const [streetError, setStreetError] = useState<string | null>(null);

	const [building, setBuilding] = useState('');
	const [buildingError, setBuildingError] = useState<string | null>(null);

	const [room, setRoom] = useState('');
	const [roomError, setRoomError] = useState<string | null>(null);

	const [code, setCode] = useState('');
	const [codeError, setCodeError] = useState<string | null>(null);

	const [post, setPost] = useState('');
	const [postError, setPostError] = useState<string | null>(null);

	const [date, setDate] = useState('');
	const [dateError, setDateError] = useState<string | null>(null);

	const [imgUrl, setImgUrl] = useState('');
	const [imgUrlError, setImgUrlError] = useState<string | null>(null);

	const [error, setError] = useState<string | null>(null);
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const { userRole } = useAuth();

	const validateForm = () => {
		let hasError = false;

		setNameError(null);
		setDescriptionError(null);
		setStreetError(null);
		setBuildingError(null);
		setRoomError(null);
		setCodeError(null);
		setPostError(null);
		setDateError(null);
		setImgUrlError(null);
		setError(null);

		if (!name) {
			setNameError('Wydarzenie musi mieć nazwę.');
			hasError = true;
		}
		if (!description) {
			setDescriptionError('Opis jest wymagany.');
			hasError = true;
		}
		if (!street) {
			setStreetError('Ulica jest wymagana.');
			hasError = true;
		}
		if (!building) {
			setBuildingError('Nr budynku jest wymagany.');
			hasError = true;
		}
		if (!code || code.length !== 6) {
			setCodeError('Kod pocztowy musi być w formacie 00-000.');
			hasError = true;
		}
		if (!post) {
			setPostError('Miasto jest wymagane.');
			hasError = true;
		}
		if (!date) {
			setDateError('Data jest wymagana.');
			hasError = true;
		}
		if (!imgUrl) {
			setImgUrlError('URL zdjęcia jest wymagany.');
			hasError = true;
		}

		if (!selectedCategory) {
			setError('Kategoria jest wymagana.');
			hasError = true;
		}

		return hasError;
	};

	const setAndCheckCode = (input: string) => {
		let formattedInput = input.replace(/[^0-9-]/g, '');

		if (formattedInput.length > 2 && !formattedInput.includes('-')) {
			formattedInput = `${formattedInput.slice(0, 2)}-${formattedInput.slice(2)}`;
		}

		if (formattedInput.length > 6) {
			formattedInput = formattedInput.slice(0, 6);
		}

		setCodeError(formattedInput.length < 6 ? 'Kod pocztowy musi być w formacie 00-000.' : null);
		setCode(formattedInput);
	};

	const fetchCategories = async () => {
		setLoading(true);
		setError(null);

		try {
			const response = await eventService.getCategories();
			setServerCategories(response.items);
			setCategories(response.items.map(category => category.name));
			setSelectedCategory(response.items[0]?.name || '');
			setLoading(false);
		} catch (err) {
			console.log('Failed to fetch categories.');
			setLoading(false);
		}
	};

	const formatDate = (date: string): string => {
		const d = new Date(date);
		const year = d.getFullYear();
		const month = String(d.getMonth() + 1).padStart(2, '0');
		const day = String(d.getDate()).padStart(2, '0');
		return `${year}-${month}-${day}`;
	};

	const fetchEvent = async (eventId: string) => {
		setLoading(true);
		setError(null);

		try {
			const event = await eventService.getEvent(eventId);

			setName(event.name || '');
			setDescription(event.description || '');
			setStreet(event.locationStreet || '');
			setBuilding(event.locationBuilding || '');
			setRoom(event.locationRoom || '');
			setAndCheckCode(event.locationCode || '');
			setPost(event.locationPost || '');
			setDate(formatDate(event.date) || '');
			setImgUrl(event.imageUrl || '');
			setSelectedCategory(event.categoryName || '');
		} catch (err) {
			console.log('Failed to fetch event details.');
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		if (userRole !== 'Admin') {
			console.log('You do not have permission to access this page.');
			return;
		}
		fetchCategories();
		if (eventId) {
			fetchEvent(eventId);
		}
	}, []);

	const handleClose = () => {
		setName('');
		setNameError(null);
		setDescription('');
		setDescriptionError(null);
		setStreet('');
		setStreetError(null);
		setBuilding('');
		setBuildingError(null);
		setRoom('');
		setRoomError(null);
		setCode('');
		setCodeError(null);
		setPost('');
		setPostError(null);
		setDate('');
		setDateError(null);
		setImgUrl('');
		setImgUrlError(null);
		setError(null);
		setShowSuccessModal(false);
		setLoading(false);
		onClose();
	};

	const handleSave = async () => {
		if (validateForm()) return;

		const token = localStorage.getItem('authToken');
		if (!token) {
			console.log('User is not authenticated.');
			return;
		}

		setLoading(true);
		setError(null);

		try {
			const category = serverCategories.find(cat => cat.name === selectedCategory);
			if (!category) {
				console.log('Category not found.');
				return;
			}
			const eventData: Event = {
				id: eventId || '',
				name: name,
				description: description,
				locationStreet: street,
				locationBuilding: building,
				locationRoom: room,
				locationCode: code,
				locationPost: post,
				date: date,
				imageUrl: imgUrl,
				categoryId: category.id,
				categoryName: category.name,
				ticketPools: {
					items: [],
				},
			};

			if (eventId) {
				await eventService.updateEvent(token, eventId, eventData);
			} else {
				await eventService.createEvent(token, eventData);
			}

			onClose();
		} catch (err) {
			console.log('Failed to save event.');
		} finally {
			setLoading(false);
		}
	};

	return {
		name,
		setName,
		nameError,
		categories,
		setCategories,
		selectedCategory,
		setSelectedCategory,
		description,
		setDescription,
		descriptionError,
		street,
		setStreet,
		streetError,
		building,
		setBuilding,
		buildingError,
		room,
		setRoom,
		roomError,
		code,
		setCode,
		codeError,
		post,
		setPost,
		postError,
		date,
		setDate,
		dateError,
		imgUrl,
		setImgUrl,
		imgUrlError,
		error,
		loading,
		showSuccessModal,
		handleClose,
		setAndCheckCode,
		handleSave,
	};
};
