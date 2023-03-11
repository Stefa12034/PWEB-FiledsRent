import { useCallback, useState } from "react";

export const useDialogController = () => {
    const [isOpen, setIsOpen] = useState(false);

    const open = useCallback(() => {
        setIsOpen(true);
    }, [setIsOpen]);

    const close = useCallback(() => {
        setIsOpen(false);
    }, [setIsOpen]);

    return {
        isOpen,
        close,
        open
    }
}