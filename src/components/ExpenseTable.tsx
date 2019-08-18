import React from 'react';

import { ExpenseRow } from './ExpenseRow';
import { Expense } from '../model/Expense';

interface ExpenseTableProps {
    expenses: Expense[];
}

const ExpenseTable : React.FC<ExpenseTableProps> = (props: ExpenseTableProps) => {
    return (
        <table>
            <thead>
                <th>Who paid</th>
                <th>For what</th>
                <th>How much</th>
                <th>Settlements</th>
            </thead>
            <tbody>
                {props.expenses.map(e => <ExpenseRow expense={e} />)}
            </tbody>
        </table>
    )
};

export {ExpenseTable};