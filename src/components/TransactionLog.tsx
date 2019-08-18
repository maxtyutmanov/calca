import React from 'react';
import { Transaction } from '../model/Transaction';
import { makeStyles, createStyles } from '@material-ui/styles';
import { Theme, Paper, TableRow, Table, TableHead, TableCell, TableBody } from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete';

interface TransactionLogProps {
    trans: Transaction[];
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
      marginTop: "15px",
      overflowX: 'auto',
    },
    table: {
      minWidth: 300,
    },
  }),
);

const TransactionLog : React.FC<TransactionLogProps> = (props) => {
    const classes = useStyles();

    return (
        <Paper className={classes.root}>
            <Table className={classes.table}>
                <TableHead>
                    <TableRow>
                        <TableCell>Contributors</TableCell>
                        <TableCell align="right">Consumers</TableCell>
                        <TableCell align="right">Description</TableCell>
                        <TableCell align="right">Amount</TableCell>
                        <TableCell></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.trans.map(tran => (
                        <TableRow>
                            <TableCell>
                                {tran.contributors.join(', ')}
                            </TableCell>
                            <TableCell align="right">
                                {tran.consumers.join(', ')}
                            </TableCell>
                            <TableCell align="right">
                                {tran.description}
                            </TableCell>
                            <TableCell align="right">
                                {tran.amount}
                            </TableCell>
                            <TableCell>
                                <DeleteIcon />
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </Paper>
    );
};

export {TransactionLog};