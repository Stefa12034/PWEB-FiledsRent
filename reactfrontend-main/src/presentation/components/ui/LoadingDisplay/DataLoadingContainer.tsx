import { Fragment } from "react";
import { DataLoadError } from "./DataLoadError";
import { DataLoading } from "./DataLoading";
import { DataLoadingPropsWithChildren } from "./DataLoading.types";

export const DataLoadingContainer = (props: DataLoadingPropsWithChildren) => {
    return <Fragment>
        {props.isError && <DataLoadError tryReload={props.tryReload} />}
        {props.isLoading && props.isError !== true && <DataLoading />}
        {props.isError !== undefined && !props.isError && props.children}
    </Fragment>
};