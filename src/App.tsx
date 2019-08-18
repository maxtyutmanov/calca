import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';

import {DudeAdd} from './components/DudeAdd';
import {TransactionAdd} from './components/TransactionAdd';
import { Transaction } from './model/Transaction';
import { TransactionLog } from './components/TransactionLog';
import { Balance } from './components/Balance';
import { DataService } from './services/DataService';

const testDudes : string[] = [
  "dan",
  "jenek",
  "tanya",
  "ksyusha",
  "rinat",
  "max"
];

const App: React.FC = () => {
  const [trans, setTrans] = useState<Transaction[]>([]);
  const [allDudes, setAllDudes] = useState<string[]>(testDudes);

  const ds = new DataService("http://localhost:5000");

  const handleTranAdd = (tran: Transaction) => {
    ds.addTran(tran).then(_ => {
      ds.getTrans("default").then(refreshedTrans => {
        setTrans(refreshedTrans);
      });
    })
  };

  ds.getTrans("default").then(refreshedTrans => setTrans(refreshedTrans));

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
