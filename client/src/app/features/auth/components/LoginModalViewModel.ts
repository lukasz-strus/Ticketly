import { useState } from 'react';
import { AuthService } from '../../../core/services/AuthService';
import { UserService } from '../../../core/services/UserService';
import { useAuth } from '../../../core/context/AuthContext';
import { useNavigate } from 'react-router';

const authService = AuthService.getInstance();
const userService = UserService.getInstance();

export const useLoginModalViewModel = (onClose: () => void) => {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [error, setError] = useState<string | null>(null);
	const [emailError, setEmailError] = useState<string | null>(null);
	const [passwordError, setPasswordError] = useState<string | null>(null);
	const [showRegisterModal, setShowRegisterModal] = useState(false);
	const { setIsLoggedIn, setUserName, setUserRole } = useAuth();
	const [showSuccessModal, setShowSuccessModal] = useState(false);
	const [loading, setLoading] = useState<boolean>(false);
	const navigate = useNavigate();

	const validateEmail = (email: string): boolean => {
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailRegex.test(email);
	};

	const handleLogin = async () => {
		setLoading(true);
		let hasError = false;

		setEmailError(null);
		setPasswordError(null);
		setError(null);

		if (!email) {
			setEmailError('Email nie może być pusty.');
			hasError = true;
		} else if (!validateEmail(email)) {
			setEmailError('NIepoprawny adres email.');
			hasError = true;
		}

		if (!password) {
			setPasswordError('Hasło nie może być puste.');
			hasError = true;
		}

		if (hasError) {
			setLoading(false);
			return;
		}

		try {
			await authService
				.loginUser(email, password)
				.then(async token => {
					localStorage.setItem('authToken', token);

					await userService
						.getMeProfile(token)
						.then(user => {
							setIsLoggedIn(true);
							const userName = user.firstName + ' ' + user.lastName;
							setUserName(userName);
							setUserRole(user.roleName);

							setShowSuccessModal(true);
						})
						.catch(error => {
							console.error('Error fetching user profile:', error);
							localStorage.removeItem('authToken');
						});
				})
				.catch(error => {
					console.error('Error fetching user profile:', error);
					localStorage.removeItem('authToken');
				});
		} catch {
			setError('Login failed. Please check your credentials.');
		} finally {
			setLoading(false);
		}
	};

	const handleClose = () => {
		setEmail('');
		setPassword('');
		setError(null);
		setEmailError(null);
		setPasswordError(null);
		onClose();
		setLoading(false);
	};

	const handleSuccessClose = () => {
		setShowSuccessModal(false);
		handleClose();
		navigate(0);
	};

	return {
		email,
		password,
		error,
		emailError,
		passwordError,
		showRegisterModal,
		loading,
		showSuccessModal,
		setEmail,
		setPassword,
		setError,
		setEmailError,
		setPasswordError,
		setShowRegisterModal,
		handleLogin,
		handleClose,
		handleSuccessClose,
	};
};
