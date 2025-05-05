import React from 'react';
import { Form, FloatingLabel } from 'react-bootstrap';

interface FormFieldProps {
	label: string;
	type: string;
	as?: 'textarea';
	placeholder?: string;
	value: string | number;
	isInvalid?: boolean;
	feedback?: string | null;
	options?: { value: string | number; label: string }[];
	floatingLabel?: boolean;
	onChange: (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
	disabled?: boolean;
}

const FormField: React.FC<FormFieldProps> = ({
	label,
	type,
	as,
	placeholder = '',
	value,
	isInvalid = false,
	feedback = null,
	options,
	floatingLabel = false,
	onChange,
	disabled = false,
}) => {
	if (floatingLabel) {
		return (
			<FloatingLabel controlId={`floating-${label}`} label={label} className='mb-3'>
				{type === 'select' && options ? (
					<Form.Select value={value} onChange={onChange} isInvalid={isInvalid}>
						{options.map(option => (
							<option key={option.value} value={option.value}>
								{option.label}
							</option>
						))}
					</Form.Select>
				) : (
					<Form.Control
						type={type}
						as={as}
						placeholder={placeholder}
						value={value}
						onChange={onChange}
						isInvalid={isInvalid}
						disabled={disabled}
					/>
				)}
				{feedback && <Form.Control.Feedback type='invalid'>{feedback}</Form.Control.Feedback>}
			</FloatingLabel>
		);
	}

	return (
		<Form.Group className='mb-3'>
			<Form.Label>{label}</Form.Label>
			{type === 'select' && options ? (
				<Form.Select value={value} onChange={onChange} isInvalid={isInvalid}>
					{options.map(option => (
						<option key={option.value} value={option.value}>
							{option.label}
						</option>
					))}
				</Form.Select>
			) : (
				<Form.Control
					type={type}
					as={as}
					placeholder={placeholder}
					value={value}
					onChange={onChange}
					isInvalid={isInvalid}
					disabled={disabled}
				/>
			)}
			{feedback && <Form.Control.Feedback type='invalid'>{feedback}</Form.Control.Feedback>}
		</Form.Group>
	);
};

export default FormField;
