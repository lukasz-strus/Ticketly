export class PasswordHelpers {
	static validatePassword = (password: string): boolean => {
		const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

		return passwordRegex.test(password);
	};

	static passwordError: string =
		'Hasło musi zawierać co najmniej 8 znaków, jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak specjalny.';

	static passwordMatchError: string = 'Hasło i potwierdzenie hasła muszą być takie same.';
}
