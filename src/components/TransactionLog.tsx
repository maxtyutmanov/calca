import React from 'react';
import { Transaction } from '../model/Transaction';
import { TransactionView } from './TransactionView';

interface TransactionLogProps {
    trans: Transaction[];
}

const TransactionLog : React.FC<TransactionLogProps> = (props) => {
    return (
        <table>
            <thead>
                <tr>
                    <th>Who paid</th>
                    <th>For what</th>
                    <th>How much</th>
                    <th>For whom</th>
                </tr>
            </thead>
            <tbody>
                {props.trans.map(tran => <TransactionView tran={tran} />)}
            </tbody>
        </table>
    );
};

export {TransactionLog};