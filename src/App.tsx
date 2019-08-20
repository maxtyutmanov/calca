import React from 'react';
import './App.css';

import {TransactionAdd} from './components/TransactionAdd';
import { Transaction } from './model/Transaction';
import { TransactionLog } from './components/TransactionLog';
import { Balance } from './components/Balance';
import { DataService } from './services/DataService';
import { CircularProgress } from '@material-ui/core';

interface AppProps
{
  collectionId: string,
  allDudes: string[]
}

interface AppState
{
  trans: Transaction[]
}

class App extends React.Component<AppProps, AppState>
{
  private ds: DataService;

  constructor(props: AppProps) {
    super(props);
    this.ds = new DataService(props.collectionId);
    this.state = {
      trans: []
    };
  }

  componentDidMount() {
    this.reloadTrans();
  }

  public render() {
    return (
      <div className="App">
          {this.state.trans && 
            <>
              <TransactionAdd allDudes={this.props.allDudes} onTranAdded={this.handleTranAdd} />
              <Balance trans={this.state.trans} />
              <TransactionLog trans={this.state.trans} onDelete={this.handleTranDelete} />
            </>}
          {!this.state.trans && <CircularProgress />}
      </div>
    );
  }

  private handleTranAdd = (tran: Transaction) => {
    this.ds.addTran(tran).then(_ => this.reloadTrans());
  };

  private handleTranDelete = (tranId: string, collectionId: string) => {
    this.ds.deleteTran(tranId).then(_ => {
      this.reloadTrans();
    });
  }

  private reloadTrans = (): Promise<void> => {
    return this.ds.getTrans().then(refreshedTrans => {
      this.setState({ ...this.state, trans: refreshedTrans });
    });
  }
}

// const App: React.FC = () => {
//   const [trans, setTrans] = useState<Transaction[] | null>(null);
//   const [allDudes, setAllDudes] = useState<string[]>(testDudes);

//   const ds = new DataService();

//   const handleTranAdd = (tran: Transaction) => {
//     ds.addTran(tran).then(refreshedTran => {
//       ds.getTrans(refreshedTran.collectionId).then(refreshedTrans => {
//         setTrans(refreshedTrans);
//       });
//     })
//   };

//   const handleTranDelete = (tranId: string, collectionId: string) => {
//     ds.deleteTran(tranId, collectionId).then(_ => {
//       ds.getTrans(collectionId).then(refreshedTrans => {
//         setTrans(refreshedTrans);
//       });
//     });
//   }

//   useEffect(() => {
//     if (!trans) {
//       ds.getTrans("default").then(refreshedTrans => setTrans(refreshedTrans));
//     }
//   }, [ds, trans, setTrans]);

  
// }

export default App;
