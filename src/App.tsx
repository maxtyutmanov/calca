import React, { useState, useEffect } from 'react';
import './App.css';

import {TransactionAdd} from './components/TransactionAdd';
import { Transaction } from './model/Transaction';
import { TransactionLog } from './components/TransactionLog';
import { Balance } from './components/Balance';
import { DataService } from './services/DataService';
import { CircularProgress } from '@material-ui/core';

const testDudes : string[] = [
  "dan",
  "jenek",
  "tanya",
  "ksyusha",
  "rinat",
  "max"
];

const App: React.FC = () => {
  const [trans, setTrans] = useState<Transaction[] | null>(null);
  const [allDudes, setAllDudes] = useState<string[]>(testDudes);

  const ds = new DataService("http://localhost:5000");

  const handleTranAdd = (tran: Transaction) => {
    ds.addTran(tran).then(refreshedTran => {
      ds.getTrans(refreshedTran.collectionId).then(refreshedTrans => {
        setTrans(refreshedTrans);
      });
    })
  };

  const handleTranDelete = (tranId: string, collectionId: string) => {
    ds.deleteTran(tranId, collectionId).then(_ => {
      ds.getTrans(collectionId).then(refreshedTrans => {
        setTrans(refreshedTrans);
      });
    });
  }

  useEffect(() => {
    if (!trans) {
      ds.getTrans("default").then(refreshedTrans => setTrans(refreshedTrans));
    }
  }, [ds, trans, setTrans]);

  return (
    <div className="App">
        {trans && 
          <>
            <TransactionAdd allDudes={allDudes} onTranAdded={handleTranAdd} />
            <Balance trans={trans} />
            <TransactionLog trans={trans} onDelete={handleTranDelete} />
          </>}
        {!trans && <CircularProgress />}
    </div>
  );
}

export default App;
