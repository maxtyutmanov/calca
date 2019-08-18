export const changeSelectedDudes = (currentDudes: string[], dude: string, isSelected: boolean) : string[] => {
    if (isSelected) {
        if (!currentDudes.includes(dude)) {
            return [...currentDudes, dude];
        }
    }
    else {
        return currentDudes.filter(d => d !== dude);
    }

    return currentDudes;
}