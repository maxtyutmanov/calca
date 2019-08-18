import React from 'react';
import { Transaction } from '../model/Transaction';
import { getBalance } from '../services/Calculation';

interface BalanceProps {
    trans: Transaction[];
};

const Balance: React.FC<BalanceProps> = (props) => {
    const balance = getBalance(props.trans);

    return (
        <div style={{textAlign: "left"}}>
            <div style={{fontWeight: "bold", fontSize: "18px"}}>Balance</div>
            {balance.map(b => <div><span>{b.dude}</span>:<span>{b.balance}</span></div>)}
        </div>
    )
};

export {Balance};