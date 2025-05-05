import React from 'react';
import { Route, BrowserRouter as Router, Routes } from 'react-router';
import MainLayout from './app/layouts/MainLayout';
import EventsPageView from './app/features/events/pages/EventsPageView';
import EventPageView from './app/features/events/pages/EventPageView';
import OrderPageView from './app/features/orders/pages/OrderPageView';
import OrderSummaryPageView from './app/features/orders/pages/OrderSummaryPageView';
import ManageCategoriesView from './app/features/admin/pages/ManageCategoriesView';
import ManageEventsView from './app/features/admin/pages/ManageEventsView';
import OrderHistoryPageView from './app/features/orders/pages/OrderHistoryPageView';

const App: React.FC = () => {
	return (
		<Router>
			<MainLayout>
				<Routes>
					<Route path='/' element={<EventsPageView />} />
					<Route path='/categories/:categoryId/events' element={<EventsPageView />} />
					<Route path='/events/:eventId' element={<EventPageView />} />
					<Route path='/order' element={<OrderPageView />} />
					<Route path='/order/summary' element={<OrderSummaryPageView />} />
					<Route path='/admin/categories' element={<ManageCategoriesView />} />
					<Route path='/admin/events' element={<ManageEventsView />} />
					<Route path='/me/orders' element={<OrderHistoryPageView />} />
				</Routes>
			</MainLayout>
		</Router>
	);
};

export default App;
