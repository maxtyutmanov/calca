export const changeSelectedDudes = (currentDudes: string[], dude: string, isSelected: boolean) : string[] => {
    if (isSelected) {
        if (!currentDudes.includes(dude)) {
            return [...currentDudes, dude];
        }
    }
    else {
        const dudeIndex = currentDudes.findIndex(d => d === dude);
        if (dudeIndex >= 0) {
            return currentDudes.splice(dudeIndex, 1);
        }
    }

    return currentDudes;
}