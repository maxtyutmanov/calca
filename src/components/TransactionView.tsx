import React from 'react';
import {Transaction} from '../model/Transaction';

interface TransactionViewProps {
    tran: Transaction;
}

const TransactionView : React.FC<TransactionViewProps> = (props) => {
    return (
        <tr>
            <td>{props.tran.contributors.join(', ')}</td>
            <td>{props.tran.description}</td>
            <td>{props.tran.amount}</td>
            <td>{props.tran.consumers.join(', ')}</td>
        </tr>
    );
};

export {TransactionView};