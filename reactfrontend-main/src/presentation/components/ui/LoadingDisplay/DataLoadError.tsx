import SyncProblemOutlinedIcon from '@mui/icons-material/SyncProblemOutlined';
import { Container, Tooltip, Typography } from "@mui/material";
import { useIntl } from 'react-intl';

export const DataLoadError = (props: { tryReload?: () => void }) => {
    const { formatMessage } = useIntl();

    return <Container>
        <Tooltip title={formatMessage({ id: "globals.refresh" })}>
            <SyncProblemOutlinedIcon sx={{ cursor: 'pointer' }} onClick={props.tryReload} fontSize="large" />
        </Tooltip>
        <Typography>{formatMessage({ id: "globals.loadingFailed" })}</Typography>
    </Container>
}