import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';

import {DudeAdd} from './components/DudeAdd';
import {ExpenseAdd} from './components/ExpenseAdd';
import {Expense} from './model/Expense';
import { ExpenseTable } from './components/ExpenseTable';

const testDudes : string[] = [
  "dan",
  "jenek",
  "tanya",
  "ksyusha",
  "rinat",
  "max"
];

const testExpenses : Expense[] = [
  { whoPaid: "max", forWhat : "Car", howMuch : 10000, settlements: [ { dude: 'max', participates: true, settled: true }, { dude: 'dan', participates: true, settled: false } ] },
  { whoPaid: "dan", forWhat : "House", howMuch : 20000, settlements: [ { dude: 'max', participates: true, settled: false }, { dude: 'dan', participates: true, settled: true } ] }
];

const App: React.FC = () => {
  const [expenses, setExpenses] = useState<Expense[]>(testExpenses);
  const [allDudes, setAllDudes] = useState<string[]>(testDudes);

  return (
    <div className="App">
      <DudeAdd />
      <ExpenseAdd allDudes={allDudes} />
      <ExpenseTable expenses={expenses} />
    </div>
  );
}

export default App;
