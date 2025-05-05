import { Container, Nav, Navbar, NavDropdown } from 'react-bootstrap';
import { Link } from 'react-router';
import UserMenuView from './components/UserMenuView';
import { EventService } from '../core/services/EventService';
import { useEffect, useState } from 'react';
import { Categories } from '../core/contracts/Event';

const eventService = EventService.getInstance();

const MainLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [categories, setCategories] = useState<Categories>();

	const getNavLinkClass = (path: string): string => {
		return location.pathname === path ? 'fw-bold' : '';
	};

	const fetchCategories = async () => {
		if (!categories) {
			const categoriesData = await eventService.getCategories();
			setCategories(categoriesData);
		}
	};

	useEffect(() => {
		fetchCategories();
	}, []);

	return (
		<>
			<Navbar expand='lg' className='fs-5 bg-light sticky-top'>
				<Container>
					<Navbar.Brand href='/'>ATUZ Projekt</Navbar.Brand>
					<Navbar.Toggle aria-controls='basic-navbar-nav' />
					<Navbar.Collapse id='basic-navbar-nav'>
						<Nav className='me-auto'>
							<NavDropdown title='Kategorie' id='categories-dropdown'>
								{categories?.items.map(category => (
									<NavDropdown.Item
										key={category.id}
										as={Link}
										to={`/categories/${category.id}/events`}
										className={getNavLinkClass(`/categories/${category.id}/events`)}>
										{category.name}
									</NavDropdown.Item>
								))}
							</NavDropdown>
						</Nav>
						<UserMenuView />
					</Navbar.Collapse>
				</Container>
			</Navbar>
			<Container>{children}</Container>
		</>
	);
};

export default MainLayout;
