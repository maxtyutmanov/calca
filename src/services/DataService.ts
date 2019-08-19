import { Transaction } from '../model/Transaction';
import moment from 'moment';

export class DataService {
    private apiUrl: string;

    constructor(apiUrl: string) {
        this.apiUrl = apiUrl;
    }

    public addTran(tran: Transaction) : Promise<Transaction> {
        return fetch(this.apiUrl + "/api/trans", {
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

    public getTrans(collectionId: string) : Promise<Transaction[]> {
        return fetch(this.apiUrl + "/api/trans/" + collectionId, {
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

    public deleteTran(tranId: string, collectionId: string) : Promise<Transaction> {
        const tran : Transaction = {
            id: "0",
            collectionId: collectionId,
            addedAt: moment().toISOString(),
            amount: 0,
            cancelsTranId: tranId,
            consumers: [],
            contributors: [],
            description: ""
        };

        return this.addTran(tran);
    }
}