export interface Transaction {
    id: string,
    contributors: string[],
    description: string,
    amount: number,
    consumers: string[],
}