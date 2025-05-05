import { Alert, Badge, Button, Card, Container, Nav, Spinner } from 'react-bootstrap';
import { useEventPageViewModel } from './EventPageViewModel';
import { useState } from 'react';
import InfoModal from '../../../core/components/InfoModal';

const EventPageView: React.FC = () => {
	const vm = useEventPageViewModel();
	const [activeTab, setActiveTab] = useState<string>('general');

	if (vm.loading)
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		);

	if (vm.error)
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Alert variant='danger'>{vm.error}</Alert>
			</Container>
		);

	if (!vm.event) return null;

	return (
		<>
			<Container className='my-5 event-container'>
				<Card className='event-img-card'>
					<Card.Img variant='top' src={vm.event?.imageUrl} alt={vm.event?.name} className='event-img' />
				</Card>
				<div className='event-info'>
					<Card className='event-detail'>
						<Card.Header>
							<Nav
								variant='tabs'
								activeKey={activeTab}
								onSelect={selectedKey => setActiveTab(selectedKey || 'general')}>
								<Nav.Item>
									<Nav.Link eventKey='general'>Ogólne</Nav.Link>
								</Nav.Item>
								<Nav.Item>
									<Nav.Link eventKey='description'>Opis</Nav.Link>
								</Nav.Item>
							</Nav>
						</Card.Header>
						<Card.Body className='event-detail-description'>
							<Card.Title className='fs-3'>
								<h1>
									<strong>{vm.event?.name}</strong>
								</h1>
							</Card.Title>
							{activeTab === 'general' && (
								<>
									<Card.Text className='fs-5'>
										<strong>Kategoria:</strong> {vm.event?.categoryName}
									</Card.Text>
									<Card.Text className='fs-5'>
										<strong>Data:</strong> {vm.dateToLocaleString(vm.event?.date)}
									</Card.Text>
									<Card.Text className='fs-5'>
										<strong>Adres:</strong>{' '}
										{vm.getAddress(
											vm.event?.locationStreet,
											vm.event?.locationBuilding,
											vm.event?.locationRoom,
											vm.event?.locationCode,
											vm.event?.locationPost
										)}
									</Card.Text>
								</>
							)}
							{activeTab === 'description' && (
								<>
									<Card.Text className='fs-5'>{vm.event?.description}</Card.Text>
								</>
							)}
							<Card.Footer className='d-flex justify-content-between align-items-center'>
								{vm.isAvailable && (
									<>
										<strong className='text-success fs-4'>
											<Badge bg='success'>W sprzedaży</Badge>
										</strong>
										<Button variant='warning' className='order-btn fs-5' onClick={vm.handleOnAddToOrder}>
											<strong>Kup bilet {vm.getPrice(vm.event?.ticketPools)}</strong>
										</Button>
									</>
								)}
								{!vm.isAvailable && (
									<strong className='text-danger fs-4'>
										<Badge bg='danger'>Wyprzedane</Badge>
									</strong>
								)}
							</Card.Footer>
						</Card.Body>
					</Card>
				</div>
			</Container>{' '}
			{vm.showInfoModal && vm.info && (
				<InfoModal show={!!vm.info} onClose={vm.handleCloseInfo} title='Info' message={vm.info} />
			)}
		</>
	);
};

export default EventPageView;
