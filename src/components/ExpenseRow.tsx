import React from 'react';
import { Expense } from '../model/Expense';

interface ExpenseRowProps {
    expense: Expense
};

const ExpenseRow: React.FC<ExpenseRowProps> = (props: ExpenseRowProps) => {
    const dudesCell = props.expense.sharingDudes.map(d => (
        <div>
            <input type="checkbox" />
            <label>{d}</label>
        </div>
    ));

    return (
        <tr>
            <td>{props.expense.whoPaid}</td>
            <td>{props.expense.forWhat}</td>
            <td>{props.expense.howMuch}</td>
            <td>{dudesCell}</td>
        </tr>);
}

export { ExpenseRow };