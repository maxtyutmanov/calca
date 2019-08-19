import {Transaction} from '../model/Transaction';
import {DudeBalance} from '../model/DudeBalance';

interface DudeHash {
    [dude: string]: number
}

const getBalance = (trans: Transaction[]) : DudeBalance[] => {
    const hash : DudeHash = {};

    const cancelledTranIds = trans.filter(t => t.cancelsTranId).map(t => t.cancelsTranId!);
    const effectiveTrans = trans.filter(t => !cancelledTranIds.includes(t.id));

    effectiveTrans.forEach(tran => {
        const contribution = tran.amount / tran.contributors.length;
        const consumption = tran.amount / tran.consumers.length;

        tran.contributors.forEach(contributor => {
            if (!Object.keys(hash).includes(contributor)) {
                hash[contributor] = contribution;
            } else {
                hash[contributor] += contribution;
            }
        });

        tran.consumers.forEach(consumer => {
            if (!Object.keys(hash).includes(consumer)) {
                hash[consumer] = -consumption;
            } else {
                hash[consumer] -= consumption;
            }
        });
    });

    return Object.keys(hash).map(dude => { 
        return { dude, balance: hash[dude] } 
    });
}

export {getBalance};