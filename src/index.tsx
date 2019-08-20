import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

const testDudes : string[] = [
  "dan",
  "jenek",
  "tanya",
  "ksyusha",
  "rinat",
  "max"
];

const query = new URLSearchParams(window.location.search);
let collectionId = query.get("cid");
if (!collectionId) {
    collectionId = "default";
}


ReactDOM.render(<App allDudes={testDudes} collectionId={collectionId!} />, document.getElementById('root'));

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
