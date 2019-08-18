import React from 'react';
import Checkbox from '@material-ui/core/Checkbox';
import { FormControlLabel, FormControl, makeStyles, createStyles, Theme, FormLabel, FormGroup } from '@material-ui/core';

interface DudeSelectorProps {
    allDudes: string[],
    header: string,
    onDudeSelectionChanged: (dude: string, isSelected: boolean) => void
};

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
    },
    formGroup: {
      display: 'flex',
      flexDirection: 'row'
    },
    formControl: {
      margin: theme.spacing(3),
    },
  }),
);

const DudeSelector : React.FC<DudeSelectorProps> = (props) => {
    const handleCheckboxChange = (dude: string, event: React.SyntheticEvent) => {
        const target = event.target as HTMLInputElement;
        props.onDudeSelectionChanged(dude, target.checked);
    };

    const classes = useStyles();

    return (
        <div className={classes.root}>
            <FormControl component="fieldset" className={classes.formControl}>
                <FormLabel component="legend">{props.header}</FormLabel>
                <FormGroup className={classes.formGroup}>
                    {props.allDudes.map(dude => (
                        <FormControlLabel
                            key={dude}
                            control={
                                <Checkbox
                                    onChange={e => handleCheckboxChange(dude, e)}
                                    color="primary"
                                    data-dude-id={dude}
                                />
                            }
                            label={dude}
                        />
                    ))}
                </FormGroup>
            </FormControl>
        </div>
    );
};

export {DudeSelector};