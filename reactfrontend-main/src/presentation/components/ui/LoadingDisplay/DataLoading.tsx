import { CircularProgress, Container, Typography } from "@mui/material";
import { useIntl } from 'react-intl';

export const DataLoading = () => {
    const { formatMessage } = useIntl();

    return <Container>
        <CircularProgress />
        <Typography>{formatMessage({ id: "globals.loading" })}</Typography>
    </Container>
}