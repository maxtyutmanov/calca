import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';

import {DudeAdd} from './components/DudeAdd';
import {TransactionAdd} from './components/TransactionAdd';
import { Transaction } from './model/Transaction';
import { TransactionLog } from './components/TransactionLog';
import { Balance } from './components/Balance';

const testDudes : string[] = [
  "dan",
  "jenek",
  "tanya",
  "ksyusha",
  "rinat",
  "max"
];

const testTrans : Transaction[] = [
  { contributors: [ "max" ], amount: 10000, description: "Car", consumers: ["max", "dan"] },
  { contributors: [ "dan" ], amount: 20000, description: "House", consumers: ["max", "dan"] }
];

const App: React.FC = () => {
  const [trans, setTrans] = useState<Transaction[]>(testTrans);
  const [allDudes, setAllDudes] = useState<string[]>(testDudes);

  const handleTranAdd = (tran: Transaction) => {
    setTrans([...trans, tran]);
  };

  return (
    <div className="App">
      <DudeAdd />
      <TransactionAdd allDudes={allDudes} onTranAdded={handleTranAdd} />
      <Balance trans={trans} />
      <TransactionLog trans={trans} />
    </div>
  );
}

export default App;
