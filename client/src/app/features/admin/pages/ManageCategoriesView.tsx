import { Alert, Button, Container, Spinner, Table } from 'react-bootstrap';
import { useManageCategoriesViewModel } from './ManageCategoriesViewModel';
import FormField from '../../../core/components/FormField';

const ManageCategoriesView: React.FC = () => {
	const vm = useManageCategoriesViewModel();
	if (vm.loading) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Spinner animation='border' />
			</Container>
		);
	}

	if (vm.error) {
		return (
			<Container className='d-flex justify-content-center align-items-center vh-100'>
				<Alert variant='danger'>{vm.error}</Alert>
			</Container>
		);
	}
	return (
		<div>
			<Container className='mb-5 content-container'>
				<h1>Zarządzaj kategoriami</h1>
				<div className='list-container'>
					<div
						className='d-flex gap-5 justify
                    -content-center flex-wrap'>
						<Container className='mt-4'>
							<FormField
								label='Nazwa kategorii'
								type='text'
								floatingLabel={true}
								value={vm.newCategoryName}
								onChange={e => vm.setNewCategoryName(e.target.value)}
							/>
							<Button className='order-btn' variant='warning' onClick={vm.handleOnCreateCategory}>
								Dodaj kategorię
							</Button>
						</Container>
						<Table bordered>
							<thead>
								<tr>
									<th colSpan={2} className='text-center'>
										Kategorie
									</th>
								</tr>
							</thead>
							<tbody>
								{vm.categories.map(category => (
									<tr key={category.id}>
										<td className='text-center align-middle'>{category.name}</td>
										<td className='text-center align-middle'>
											<Button variant='danger' onClick={() => vm.handleOnDeleteCategory(category.id)}>
												Usuń
											</Button>
										</td>
									</tr>
								))}
							</tbody>
						</Table>
					</div>
				</div>
			</Container>
		</div>
	);
};

export default ManageCategoriesView;
