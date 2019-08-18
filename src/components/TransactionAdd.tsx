import React, { createRef } from 'react';
import {Transaction} from '../model/Transaction';
import { DudeSelector } from './DudeSelector';
import { changeSelectedDudes } from '../services/Utils';
import Button from '@material-ui/core/Button';
import { FormControl, InputLabel, Input, FormHelperText, makeStyles, Theme, createStyles, Paper } from '@material-ui/core';

interface TransactionAddProps {
    allDudes: string[],
    onTranAdded: (tran: Transaction) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
      marginTop: "15px",
      marginBottom: "15px",
      padding: "15px",
      overflowX: 'auto',
    },
    addButton: {
      width: '100%',
    },
  }),
);

const TransactionAdd: React.FC<TransactionAddProps> = (props) => {
    const [contributors, setContributors] = React.useState<string[]>([]);
    const [consumers, setConsumers] = React.useState<string[]>([]);
    const [amount, setAmount] = React.useState<number>(0);
    const [description, setDescription] = React.useState<string>("");
    const classes = useStyles();

    const handleContributorsChange = (dude: string, isSelected: boolean) => {
        const newContributors = changeSelectedDudes(contributors, dude, isSelected);
        console.log(newContributors);
        setContributors(newContributors);
    }

    const handleConsumersChange = (dude: string, isSelected: boolean) => {
        const newConsumers = changeSelectedDudes(consumers, dude, isSelected);
        console.log(newConsumers);
        setConsumers(newConsumers);
    }

    const onSubmit = (event: React.SyntheticEvent) => {
        const tran : Transaction = {
            contributors,
            description,
            amount,
            consumers
        };

        props.onTranAdded(tran);
        event.preventDefault();
    }

    return (
        <Paper className={classes.root}>
            <FormControl component="fieldset">
                <FormControl>
                    <InputLabel htmlFor="description">Description</InputLabel>
                    <Input id="description" aria-describedby="description-text" onChange={e => setDescription(e.target.value)} />
                    <FormHelperText id="description-text">What is it that you pay for</FormHelperText>
                </FormControl>
                <FormControl>
                    <InputLabel htmlFor="amount">Amount</InputLabel>
                    <Input id="amount" aria-describedby="description-text" type="number" onChange={e => setAmount(parseInt(e.target.value, 10))} />
                    <FormHelperText id="amount-text">How much (roubles please)</FormHelperText>
                </FormControl>
                <DudeSelector header="Contributors" allDudes={props.allDudes} onDudeSelectionChanged={handleContributorsChange} />
                <DudeSelector header="Consumers" allDudes={props.allDudes} onDudeSelectionChanged={handleConsumersChange} />
                <Button className={classes.addButton} variant="contained" color="primary" onClick={onSubmit}>Add</Button>
            </FormControl>
        </Paper>
    );
}

export { TransactionAdd };