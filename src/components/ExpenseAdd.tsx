import React from 'react';

interface ExpenseAddProps {
    allDudes: string[]
}

const ExpenseAdd: React.FC<ExpenseAddProps> = (props) => {
    const dudeSharing = props.allDudes.map(dude => (
        <div style={{float: "left", marginLeft: "5px"}}>
            <label>{dude}</label>
            <input type="checkbox" />
        </div>
    ));

    return (
        <div>
            <form style={{textAlign: "left"}}>
                <div>
                    <label style={{margin: "2px"}}>Who paid</label>
                    <input type="text" required />
                </div>
                <div>
                    <label style={{margin: "2px"}}>For what</label>
                    <input type="text" required />
                </div>
                <div>
                    <label style={{margin: "2px"}}>How much</label>
                    <input type="text" required />
                </div>
                <div>
                    <label style={{float: "left"}}>Who shares: </label>
                    {dudeSharing}
                </div>
                <div style={{clear: "both"}}>
                    <button>Add this expense</button>
                </div>
            </form>
        </div>
    )
}

export { ExpenseAdd };