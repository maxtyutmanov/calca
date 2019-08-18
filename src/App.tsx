import React, { useState } from 'react';
import logo from './logo.svg';
import './App.css';

import {DudeAdd} from './components/DudeAdd';
import {ExpenseAdd} from './components/ExpenseAdd';
import {Expense} from './model/Expense';
import { ExpenseTable } from './components/ExpenseTable';

const testExpenses : Expense[] = [
  { forWhat : "Car", howMuch : 10000, sharingDudes : [ "max", "dan" ], whoPaid : [ "max" ] }
];

const App: React.FC = () => {
  const [expenses, setExpenses] = useState<Expense[]>(testExpenses);

  return (
    <div className="App">
      <DudeAdd />
      <ExpenseAdd />
      <ExpenseTable expenses={expenses} />
    </div>
  );
}

export default App;
