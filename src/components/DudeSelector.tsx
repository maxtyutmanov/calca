import React from 'react';

interface DudeSelectorProps {
    allDudes: string[],
    onDudeSelectionChanged: (dude: string, isSelected: boolean) => void
};

const DudeSelector : React.FC<DudeSelectorProps> = (props) => {
    const handleCheckboxChange = (event: React.SyntheticEvent) => {
        const target = event.target as HTMLInputElement;
        const dudeAttr = target.attributes.getNamedItem("data-dude-id");
        if (dudeAttr != null) {
            const dude = dudeAttr.value;
            props.onDudeSelectionChanged(dude, target.checked);
        }
    }

    return (
        <div>
            <div style={{clear: "both"}}></div>
            {props.allDudes.map(dude => (
                <div key={dude} style={{float: "left", marginRight: "10px"}}>
                    <label>{dude}</label>
                    <input type="checkbox" data-dude-id={dude} onChange={handleCheckboxChange} />
                </div>
            ))}
            <div style={{clear: "both"}}></div>
        </div>
    );
};

export {DudeSelector};