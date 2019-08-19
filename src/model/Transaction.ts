export interface Transaction {
    id: string,
    collectionId: string,
    addedAt: string,
    contributors: string[],
    description: string,
    amount: number,
    consumers: string[],
    cancelsTranId: string|undefined
}