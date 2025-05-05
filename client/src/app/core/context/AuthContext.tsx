import React, { createContext, useState, useContext } from 'react';

interface AuthContextProps {
	isLoggedIn: boolean;
	setIsLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
	userName: string | null;
	setUserName: React.Dispatch<React.SetStateAction<string | null>>;
	userRole: string | null;
	setUserRole: React.Dispatch<React.SetStateAction<string | null>>;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
	const [userName, setUserName] = useState<string | null>('Guest');
	const [userRole, setUserRole] = useState<string | null>('Guest');

	return (
		<AuthContext.Provider value={{ isLoggedIn, setIsLoggedIn, userName, setUserName, userRole, setUserRole }}>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = () => {
	const context = useContext(AuthContext);
	if (!context) {
		throw new Error('useAuth must be used within an AuthProvider');
	}
	return context;
};
