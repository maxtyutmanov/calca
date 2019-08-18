export interface Transaction {
    contributors: string[],
    description: string,
    amount: number,
    consumers: string[],
}