export class EmailHelpers {
	static validateEmail = (email: string): boolean => {
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailRegex.test(email);
	};

	static emailError: string = 'Błędny adres email.';

	static emailMatchError: string = 'Podane adresy email nie są takie same.';
}
