import React, { useEffect, useCallback, useMemo } from 'react';
import { Transaction } from '../model/Transaction';
import { makeStyles, createStyles } from '@material-ui/styles';
import { Theme, Paper, TableRow, Table, TableHead, TableCell, TableBody } from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete';
import moment from 'moment';

interface TransactionLogProps {
    trans: Transaction[];
    onDelete: (tranId: string, collectionId: string) => void;
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
    cancelledTran: {
      textDecoration: "line-through"
    },
    normalTran: {}
  }),
);

const TransactionLog : React.FC<TransactionLogProps> = (props) => {
    const classes = useStyles();

    const handleDeleteClick = (tranId: string, collectionId: string) => {
        props.onDelete(tranId, collectionId);
    }

    const cancelledTranIds = useMemo(() => {
        return props.trans.filter(t => t.cancelsTranId).map(t => t.cancelsTranId!);
    }, [props.trans]);

    const renderTran = (tran: Transaction) => {
        if (tran.cancelsTranId) {
            return (
                <TableRow key={tran.id}>
                    <TableCell>
                        {tran.id}
                    </TableCell>
                    <TableCell>{moment(tran.addedAt).format("DD.MM.YYYY HH:mm")}</TableCell>
                    <TableCell colSpan={5}>Cancels transaction {tran.cancelsTranId}</TableCell>
                </TableRow>
            );
        }
        else {
            const isCancelled = cancelledTranIds.includes(tran.id);

            return (
                <TableRow key={tran.id} className={isCancelled ? classes.cancelledTran : classes.normalTran}>
                    <TableCell>
                        {tran.id}
                    </TableCell>
                    <TableCell>
                        {moment(tran.addedAt).format("DD.MM.YYYY HH:mm")}
                    </TableCell>
                    <TableCell align="right">
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
                        <DeleteIcon onClick={() => handleDeleteClick(tran.id, tran.collectionId)} />
                    </TableCell>
                </TableRow>
            );
        }
    }

    return (
        <Paper className={classes.root}>
            <Table className={classes.table}>
                <TableHead>
                    <TableRow>
                        <TableCell>#</TableCell>
                        <TableCell align="right">Created at</TableCell>
                        <TableCell align="right">Contributors</TableCell>
                        <TableCell align="right">Consumers</TableCell>
                        <TableCell align="right">Description</TableCell>
                        <TableCell align="right">Amount</TableCell>
                        <TableCell></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.trans.map(tran => renderTran(tran))}
                </TableBody>
            </Table>
        </Paper>
    );
};

export {TransactionLog};