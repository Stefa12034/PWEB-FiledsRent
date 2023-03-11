import {
    Button,
    CircularProgress,
    FormControl,
    FormHelperText,
    FormLabel,
    Grid,
    Stack,
    OutlinedInput,
    Select,
    MenuItem
} from "@mui/material";
import { FormattedMessage, useIntl } from "react-intl";
import { useUserAddFormController } from "./UserAddForm.controller";
import { isEmpty, isUndefined } from "lodash";
import { UserRoleEnum } from "@infrastructure/apis/client";

export const UserAddForm = (props: { onSubmit?: () => void }) => {
    const { formatMessage } = useIntl();
    const { state, actions, computed } = useUserAddFormController(props.onSubmit);

    return <form onSubmit={actions.handleSubmit(actions.submit)}>
        <Stack spacing={4} style={{ width: "100%" }}>
            <Grid container item direction="row" xs={12} columnSpacing={4}>
                <Grid container item direction="column" xs={6} md={6}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.name)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.name" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("name")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: formatMessage({
                                        id: "globals.name",
                                    }),
                                })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.name)}
                        >
                            {state.errors.name?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={6}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.email)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.email" />
                        </FormLabel>
                        <OutlinedInput
                            {...actions.register("email")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: formatMessage({
                                        id: "globals.email",
                                    }),
                                })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.email)}
                        >
                            {state.errors.email?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={6}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.password)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.password" />
                        </FormLabel>
                        <OutlinedInput
                            type="password"
                            {...actions.register("password")}
                            placeholder={formatMessage(
                                { id: "globals.placeholders.textInput" },
                                {
                                    fieldName: formatMessage({
                                        id: "globals.password",
                                    }),
                                })}
                            autoComplete="none"
                        />
                        <FormHelperText
                            hidden={isUndefined(state.errors.password)}
                        >
                            {state.errors.password?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
                <Grid container item direction="column" xs={6} md={6}>
                    <FormControl
                        fullWidth
                        error={!isUndefined(state.errors.role)}
                    >
                        <FormLabel required>
                            <FormattedMessage id="globals.role" />
                        </FormLabel>
                        <Select
                            {...actions.register("role")}
                            value={actions.watch("role")}
                            onChange={actions.selectRole}
                            displayEmpty
                        >
                            <MenuItem value="" disabled>
                                <span className="text-gray">
                                    {formatMessage({ id: "globals.placeholders.selectInput" }, {
                                        fieldName: formatMessage({
                                            id: "globals.role",
                                        }),
                                    })}
                                </span>
                            </MenuItem>
                            <MenuItem value={UserRoleEnum.Client}>
                                <FormattedMessage id="globals.client" />
                            </MenuItem>
                            <MenuItem value={UserRoleEnum.Personnel}>
                                <FormattedMessage id="globals.personnel" />
                            </MenuItem>
                            <MenuItem value={UserRoleEnum.Admin}>
                                <FormattedMessage id="globals.admin" />
                            </MenuItem>
                        </Select>
                        <FormHelperText
                            hidden={isUndefined(state.errors.role)}
                        >
                            {state.errors.role?.message}
                        </FormHelperText>
                    </FormControl>
                </Grid>
            </Grid>
            <Grid container item direction="row" xs={12} className="padding-top-sm">
                <Grid container item direction="column" xs={12} md={7}></Grid>
                <Grid container item direction="column" xs={5}>
                    <Button type="submit" disabled={!isEmpty(state.errors) || computed.isSubmitting}>
                        {!computed.isSubmitting && <FormattedMessage id="globals.submit" />}
                        {computed.isSubmitting && <CircularProgress />}
                    </Button>
                </Grid>
            </Grid>
        </Stack>
    </form>
};