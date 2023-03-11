import { useTableController } from "../Table.controller";
import { useUserApi } from "@infrastructure/apis/api-management";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useCallback } from "react";
import { usePaginationController } from "../Pagination.controller";

export const useUserTableController = () => {
    const { getUsers: { key: queryKey, query }, deleteUser: { key: deleteUserKey, mutation: deleteUser } } = useUserApi();
    const queryClient = useQueryClient();
    const { page, pageSize, setPagination } = usePaginationController();
    const { data, isError, isLoading } = useQuery([queryKey, page, pageSize], () => query({ page, pageSize }));
    const { mutateAsync: deleteMutation } = useMutation([deleteUserKey], deleteUser);
    const remove = useCallback(
        (id: string) => deleteMutation(id).then(() => queryClient.invalidateQueries([queryKey])),
        [queryClient, deleteMutation, queryKey]);

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
        remove
    };
}