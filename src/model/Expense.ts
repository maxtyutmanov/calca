import { DudeSettlementInfo } from './DudeSettlementInfo';

export interface Expense {
    whoPaid: string;
    forWhat: string;
    howMuch: number;
    settlements: DudeSettlementInfo[];
};