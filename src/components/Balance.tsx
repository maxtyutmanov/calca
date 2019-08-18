import React from 'react';
import { Transaction } from '../model/Transaction';
import { getBalance } from '../services/Calculation';
import { makeStyles, createStyles } from '@material-ui/styles';
import { Theme, Paper, TableRow, Table, TableHead, TableCell, TableBody } from '@material-ui/core';

interface BalanceProps {
    trans: Transaction[];
};

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

const Balance: React.FC<BalanceProps> = (props) => {
    const balance = getBalance(props.trans);
    const classes = useStyles();

    return (
        <Paper className={classes.root}>
            <Table className={classes.table}>
                <TableHead>
                    <TableRow>
                        <TableCell>Dude</TableCell>
                        <TableCell align="right">Balance</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {balance.map(row => (
                        <TableRow key={row.dude}>
                            <TableCell component="th" scope="row">
                                {row.dude}
                            </TableCell>
                            <TableCell align="right">
                                {row.balance}
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </Paper>
    )

    // return (
    //     <div style={{textAlign: "left"}}>
    //         <div style={{fontWeight: "bold", fontSize: "18px"}}>Balance</div>
    //         {balance.map(b => <div><span>{b.dude}</span>:<span>{b.balance}</span></div>)}
    //     </div>
    // )
};

export {Balance};