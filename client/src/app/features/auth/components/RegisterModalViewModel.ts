import { useState } from 'react';
import { AuthService } from '../../../core/services/AuthService';
import { EmailHelpers } from '../../../helpers/EmailHelpers';
import { PasswordHelpers } from '../../../helpers/PasswordHelpers';

const authService = AuthService.getInstance();

export const useRegisterModalViewModel = (onClose: () => void) => {
	const [email, setEmail] = useState('');
	const [confirmEmail, setConfirmEmail] = useState('');
	const [emailError, setEmailError] = useState<string | null>(null);

	const [password, setPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [passwordError, setPasswordError] = useState<string | null>(null);

	const [firstName, setFirstName] = useState('');
	const [firstNameError, setFirstNameError] = useState<string | null>(null);

	const [lastName, setLastName] = useState('');
	const [lastNameError, setLastNameError] = useState<string | null>(null);

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

	const [error, setError] = useState<string | null>(null);
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);

	const validateForm = () => {
		let hasError = false;

		setEmailError(null);
		setPasswordError(null);
		setError(null);
		setFirstNameError(null);
		setLastNameError(null);
		setStreetError(null);
		setBuildingError(null);
		setRoomError(null);
		setCodeError(null);
		setPostError(null);

		if (!email || !EmailHelpers.validateEmail(email)) {
			setEmailError(EmailHelpers.emailError);
			hasError = true;
		} else if (email !== confirmEmail) {
			setEmailError(EmailHelpers.emailMatchError);
			hasError = true;
		}

		if (!password || !PasswordHelpers.validatePassword(password)) {
			setPasswordError(PasswordHelpers.passwordError);
			hasError = true;
		} else if (password !== confirmPassword) {
			setPasswordError(PasswordHelpers.passwordMatchError);
			hasError = true;
		}

		if (!firstName) {
			setFirstNameError('Imię nie może być puste.');
			hasError = true;
		}
		if (!lastName) {
			setLastNameError('Nazwisko nie może być puste.');
			hasError = true;
		}

		if (!street) {
			setStreetError('Ulica nie może być pusta.');
			hasError = true;
		}
		if (!building) {
			setBuildingError('Budynek nie może być pusty.');
			hasError = true;
		}
		if (!code || code.length !== 6) {
			setCodeError('Kod pocztowy musi być w formacie 00-000.');
			hasError = true;
		}
		if (!post) {
			setPostError('Miasto nie może być puste.');
			hasError = true;
		}

		return !hasError;
	};

	const handleRegister = async () => {
		if (!validateForm()) return;

		setLoading(true);

		try {
			await authService.registerUser(
				email,
				password,
				firstName,
				lastName,
				street,
				building,
				room,
				code.replace('-', ''),
				post
			);

			setShowSuccessModal(true);
		} catch {
			setError('Rejestracja nie powiodła się. Sprawdź poprawność danych.');
		} finally {
			setLoading(false);
		}
	};

	const handleClose = () => {
		setEmail('');
		setConfirmEmail('');
		setPassword('');
		setConfirmPassword('');
		setError(null);
		setEmailError(null);
		setPasswordError(null);
		onClose();
		setLoading(false);
	};

	const handleSuccessClose = () => {
		setShowSuccessModal(false);
		handleClose();
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

	return {
		email,
		setEmail,
		confirmEmail,
		setConfirmEmail,
		emailError,
		password,
		setPassword,
		confirmPassword,
		setConfirmPassword,
		passwordError,
		firstName,
		setFirstName,
		firstNameError,
		lastName,
		setLastName,
		lastNameError,
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
		error,
		loading,
		showSuccessModal,
		handleRegister,
		handleClose,
		handleSuccessClose,
		setAndCheckCode,
	};
};
