import React from 'react';
import { Expense } from '../model/Expense';

interface ExpenseRowProps {
    expense: Expense
};

const ExpenseRow: React.FC<ExpenseRowProps> = (props: ExpenseRowProps) => {
    const dudesCell = props.expense.settlements.filter(s => s.participates).map(s => (
        <div>
            <input type="checkbox" checked={s.dude === props.expense.whoPaid || s.settled} disabled={s.dude === props.expense.whoPaid} />
            <label>{s.dude}</label>
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