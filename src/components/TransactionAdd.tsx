import React, { createRef } from 'react';
import {Transaction} from '../model/Transaction';
import { DudeSelector } from './DudeSelector';
import { changeSelectedDudes } from '../services/Utils';

interface TransactionAddProps {
    allDudes: string[],
    onTranAdded: (tran: Transaction) => void;
}

const TransactionAdd: React.FC<TransactionAddProps> = (props) => {
    const descriptionInput = createRef<HTMLInputElement>();
    const amountInput = createRef<HTMLInputElement>();

    const [contributors, setContributors] = React.useState<string[]>([]);
    const [consumers, setConsumers] = React.useState<string[]>([]);

    const handleContributorsChange = (dude: string, isSelected: boolean) => {
        const newContributors = changeSelectedDudes(contributors, dude, isSelected);
        setContributors(newContributors);
    }

    const handleConsumersChange = (dude: string, isSelected: boolean) => {
        const newConsumers = changeSelectedDudes(consumers, dude, isSelected);
        setConsumers(newConsumers);
    }

    const onSubmit = (event: React.SyntheticEvent) => {
        if (descriptionInput.current && amountInput.current) {
            // TODO: error handling
            
            const description = descriptionInput.current.value;
            const amount = parseInt(amountInput.current.value, 10);

            const tran : Transaction = {
                contributors,
                description,
                amount,
                consumers
            };

            props.onTranAdded(tran);

            descriptionInput.current.value = "";
            amountInput.current.value = "";
        }

        event.preventDefault();
    }

    return (
        <div>
            <form onSubmit={onSubmit} style={{textAlign: "left"}}>
                <div>
                    <label style={{margin: "2px", fontWeight: "bold"}}>Contributors</label>
                    <DudeSelector allDudes={props.allDudes} onDudeSelectionChanged={handleContributorsChange} />
                </div>
                <div>
                    <label style={{marginRight: "5px", fontWeight: "bold"}}>Description</label>
                    <input type="text" required ref={descriptionInput} />
                </div>
                <div>
                    <label style={{marginRight: "5px", fontWeight: "bold"}}>Amount</label>
                    <input type="text" required ref={amountInput} />
                </div>
                <div>
                    <label style={{float: "left", fontWeight: "bold"}}>Consumers</label>
                    <DudeSelector allDudes={props.allDudes} onDudeSelectionChanged={handleConsumersChange} />
                </div>
                <div style={{clear: "both"}}>
                    <button>Add this expense</button>
                </div>
            </form>
        </div>
    )
}

export { TransactionAdd };