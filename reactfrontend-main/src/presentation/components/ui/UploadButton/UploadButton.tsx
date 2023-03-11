import { Button, CircularProgress } from "@mui/material";
import CloudUploadOutlinedIcon from '@mui/icons-material/CloudUploadOutlined';
import { useCallback } from "react";
import { UploadButtonProps } from "./UploadButton.types";

export const UploadButton = (props: UploadButtonProps) => {
    const onUpload = useCallback((event: React.ChangeEvent<HTMLInputElement>) => {
        if (!event.target.files || !event.target.files[0]) {
            return;
        }

        props.onUpload(event.target.files[0]);
    }, [props.onUpload]);

    return <Button color="primary" variant="contained" component="label" disabled={props.disabled}>
        <CloudUploadOutlinedIcon fontSize="small" style={{ margin: "0.25rem" }} />
        {props.isLoading && <CircularProgress />}
        {props.text}
        <input hidden accept={props.acceptFileType} type="file" onChange={onUpload} />
    </Button>;
};