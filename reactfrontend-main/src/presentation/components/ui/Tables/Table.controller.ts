import { LabelDisplayedRowsArgs } from "@mui/material";
import { ChangeEvent, MouseEvent, useCallback } from "react";
import { useIntl } from "react-intl";

export const useTableController = (onPaginationChange: (page: number, pageSize: number) => void, defaultPageSize?: number) => {
    const { formatMessage } = useIntl();
    const handleChangePage = useCallback((
        _event: MouseEvent<HTMLButtonElement> | null,
        newPage: number,
    ) => {
        onPaginationChange(newPage + 1, defaultPageSize ?? 10);
    }, [onPaginationChange, defaultPageSize]);

    const handleChangePageSize = useCallback((
        event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    ) => {
        onPaginationChange(1, parseInt(event.target.value, 10));
    }, [onPaginationChange]);

    const labelDisplay = useCallback(({ to, from, count }: LabelDisplayedRowsArgs) => {
        return count !== -1 ?
            formatMessage({ id: "labels.paginationLabelNormal" }, { to, from, count }) :
            formatMessage({ id: "labels.paginationLabelOverflow" }, { to, from });
    }, [formatMessage]);

    return {
        labelDisplay,
        handleChangePage,
        handleChangePageSize
    };
}