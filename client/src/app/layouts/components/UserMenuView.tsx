import { Button, Nav, NavDropdown } from 'react-bootstrap';
import { Link } from 'react-router';
import LoginModalView from '../../features/auth/components/LoginModalView';
import InfoModal from '../../core/components/InfoModal';
import { useUserMenuViewModel } from './UserMenuViewModel';

const UserMenuView: React.FC = () => {
	const vm = useUserMenuViewModel();

	return (
		<>
			<Nav>
				<Nav.Item className='me-3'>
					{vm.userRole !== 'Admin' && (
						<Nav.Link as={Link} to='/order' className={vm.getNavLinkClass('/order')}>
							Koszyk
						</Nav.Link>
					)}
				</Nav.Item>
				{vm.isLoggedIn ? (
					<NavDropdown title={vm.userName || 'Guest'} id='user-dropdown'>
						{vm.userRole === 'Admin' && (
							<NavDropdown.Item as={Link} to='/admin/events' className={vm.getNavLinkClass('/admin/events')}>
								Zarządzaj wydarzeniami
							</NavDropdown.Item>
						)}
						{vm.userRole === 'Admin' && (
							<NavDropdown.Item as={Link} to='/admin/categories' className={vm.getNavLinkClass('/admin/categories')}>
								Zarządzaj kategoriami
							</NavDropdown.Item>
						)}
						{vm.userRole !== 'Admin' && (
							<NavDropdown.Item as={Link} to='/me/orders' className={vm.getNavLinkClass('/me/orders')}>
								Historia zamówień
							</NavDropdown.Item>
						)}
						<NavDropdown.Item onClick={vm.handleLogout}>Wyloguj</NavDropdown.Item>{' '}
					</NavDropdown>
				) : (
					<Button variant='outline-dark' onClick={() => vm.setShowLoginModal(true)}>
						Zaloguj
					</Button>
				)}
			</Nav>
			<LoginModalView show={vm.showLoginModal} onClose={() => vm.handleLogin()} />

			{vm.showSuccessModal && (
				<InfoModal show={vm.showSuccessModal} onClose={vm.handleSuccessClose} title='Zostałeś wylogowany' />
			)}
		</>
	);
};

export default UserMenuView;
