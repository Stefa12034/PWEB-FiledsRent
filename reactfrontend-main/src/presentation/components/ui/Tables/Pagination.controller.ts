import { useCallback, useState } from "react";

export const usePaginationController = () => {
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const setPagination = useCallback((newPage: number, newPageSize: number) => {
        setPage(newPage);
        setPageSize(newPageSize);
    }, [setPage, setPageSize]);

    return {
        page,
        pageSize,
        setPage,
        setPageSize,
        setPagination
    }
}