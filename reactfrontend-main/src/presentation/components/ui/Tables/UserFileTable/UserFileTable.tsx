import { useIntl } from "react-intl";
import { isUndefined } from "lodash";
import { IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from "@mui/material";
import { DataLoadingContainer } from "../../LoadingDisplay";
import { useUserFileTableController } from "./UserFileTable.controller";
import { UserFileDTO } from "@infrastructure/apis/client";
import CloudDownloadIcon from '@mui/icons-material/CloudDownload';
import FileOpenIcon from '@mui/icons-material/FileOpen';
import { dateToDateStringOrNull } from "@infrastructure/utils/dateUtils";
import { UserFileAddDialog } from "../../Dialogs/UserFileAddDialog";

const useHeader = (): { key: keyof UserFileDTO, name: string }[] => {
    const { formatMessage } = useIntl();

    return [
        { key: "name", name: formatMessage({ id: "globals.name" }) },
        { key: "description", name: formatMessage({ id: "globals.description" }) },
        { key: "user", name: formatMessage({ id: "globals.addBy" }) },
        { key: "createdAt", name: formatMessage({ id: "globals.createdAt" }) }
    ]
};

const getRowValues = (entries: UserFileDTO[] | null | undefined, orderMap: { [key: string]: number }) =>
    entries?.map(
        entry => {
            return {
                entry: entry,
                data: Object.entries(entry).filter(([e]) => !isUndefined(orderMap[e])).sort(([a], [b]) => orderMap[a] - orderMap[b]).map(([key, value]) => { return { key, value } })
            }
        });

const renders: { [key: string]: (value: any) => string | null } = {
    createdAt: dateToDateStringOrNull,
    user: (value) => value.name
};

export const UserFileTable = () => {
    const { formatMessage } = useIntl();
    const header = useHeader();
    const orderMap = header.reduce((acc, e, i) => { return { ...acc, [e.key]: i } }, {}) as { [key: string]: number };
    const headerValues = [...header].sort((a, b) => orderMap[a.key] - orderMap[b.key]);
    const { handleChangePage, handleChangePageSize, pagedData, isError, isLoading, tryReload, labelDisplay, downloadUserFile, openUserFile } = useUserFileTableController();
    const rowValues = getRowValues(pagedData?.data, orderMap);

    return <DataLoadingContainer isError={isError} isLoading={isLoading} tryReload={tryReload}>
        <UserFileAddDialog />
        {!isUndefined(pagedData) && !isUndefined(pagedData?.totalCount) && !isUndefined(pagedData?.page) && !isUndefined(pagedData?.pageSize) &&
            <TablePagination
                component="div"
                count={pagedData.totalCount}
                page={pagedData.totalCount !== 0 ? pagedData.page - 1 : 0}
                onPageChange={handleChangePage}
                rowsPerPage={pagedData.pageSize}
                onRowsPerPageChange={handleChangePageSize}
                labelRowsPerPage={formatMessage({ id: "labels.itemsPerPage" })}
                labelDisplayedRows={labelDisplay}
                showFirstButton
                showLastButton
            />}

        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        {
                            headerValues.map(e => <TableCell key={`header_${String(e.key)}`}>{e.name}</TableCell>)
                        }
                        <TableCell>{formatMessage({ id: "labels.actions" })}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {
                        rowValues?.map(({ data, entry }, rowIndex) => <TableRow key={`row_${rowIndex + 1}`}>
                            {data.map((keyValue, index) => {
                                return <TableCell key={`cell_${rowIndex + 1}_${index + 1}`}>{isUndefined(renders[keyValue.key]) ? keyValue.value : renders[keyValue.key](keyValue.value)}</TableCell>
                            })}
                            <TableCell>
                                {<IconButton color="primary" onClick={() => downloadUserFile(entry)}>
                                    <CloudDownloadIcon color="primary" fontSize='small' />
                                </IconButton>}
                                {entry.name?.endsWith(".pdf") && <IconButton color="primary" onClick={() => openUserFile(entry)}>
                                    <FileOpenIcon color="primary" fontSize='small' />
                                </IconButton>}
                            </TableCell>
                        </TableRow>)
                    }
                </TableBody>
            </Table>
        </TableContainer>
    </DataLoadingContainer >
}