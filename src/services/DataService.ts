import { Transaction } from '../model/Transaction';

export class DataService {
    private collectionId: string;
    constructor(collectionId: string) {
        this.collectionId = collectionId;
    }

    public addTran(tran: Transaction) : Promise<Transaction> {
        tran.collectionId = this.collectionId;
        return fetch("/api/trans", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(tran)
        }).then(response => {
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            return response.json() as Promise<Transaction>;
        });
    }

    public getTrans() : Promise<Transaction[]> {
        return fetch(`/api/trans/${this.collectionId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            },
        }).then(response => {
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            return response.json() as Promise<Transaction[]>;
        });
    }

    public deleteTran(tranId: string) : Promise<Transaction> {
        return fetch(`/api/trans/${this.collectionId}/${tranId}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json'
            }
        }).then(response => {
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            return response.json() as Promise<Transaction>;
        });
    }
}