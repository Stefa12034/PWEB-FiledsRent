import { useTableController } from "../Table.controller";
import { useUserFileApi } from "@infrastructure/apis/api-management";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import { usePaginationController } from "../Pagination.controller";
import { downloadDocument, openDocument } from "@infrastructure/utils/downloadUtils";
import { UserFileDTO } from "@infrastructure/apis/client";

const getFileContent = (filename?: string) => {
    if (!filename) {
        return;
    }

    if (filename.endsWith(".pdf")) {
        return "application/pdf";
    }
}

export const useUserFileTableController = () => {
    const { getUserFiles: { key: queryKey, query }, downloadUserFile: { query: download } } = useUserFileApi();
    const queryClient = useQueryClient();
    const { page, pageSize, setPagination } = usePaginationController();
    const { data, isError, isLoading } = useQuery([queryKey, page, pageSize], () => query({ page, pageSize }));
    const downloadUserFile = useCallback((userFile: UserFileDTO) => download(userFile.id ?? '').then((data) => downloadDocument(data, userFile.name ?? '')), [download]);
    const openUserFile = useCallback((userFile: UserFileDTO) => download(userFile.id ?? '').then((data) => userFile.name?.endsWith(".pdf") ? openDocument(data, getFileContent(userFile.name)) : openDocument(data)), [download]);

    const tryReload = useCallback(
        () => queryClient.invalidateQueries([queryKey]),
        [queryClient, queryKey]);

    const tableController = useTableController(setPagination, data?.response?.pageSize);

    return {
        ...tableController,
        tryReload,
        pagedData: data?.response,
        isError,
        isLoading,
        downloadUserFile,
        openUserFile
    };
}