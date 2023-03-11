import { UserAddFormController, UserAddFormModel } from "./UserAddForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useUserApi } from "@infrastructure/apis/api-management";
import { useCallback } from "react";
import { UserRoleEnum } from "@infrastructure/apis/client";
import { SelectChangeEvent } from "@mui/material";

const getDefaultValues = (initialData?: UserAddFormModel) => {
    const defaultValues = {
        email: "",
        name: "",
        password: "",
        role: "" as UserRoleEnum
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

const useInitUserAddForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
        name: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.name",
                    }),
                }))
            .default(defaultValues.name),
        email: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.email",
                    }),
                }))
            .email()
            .default(defaultValues.email),
        password: yup.string()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.password",
                    }),
                })),
        role: yup.string()
            .oneOf([
                UserRoleEnum.Admin,
                UserRoleEnum.Personnel,
                UserRoleEnum.Client
            ])
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.role",
                    }),
                }))
            .default(defaultValues.role)
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

export const useUserAddFormController = (onSubmit?: () => void): UserAddFormController => {
    const { defaultValues, resolver } = useInitUserAddForm();
    const { addUser: { mutation, key: mutationKey }, getUsers: { key: queryKey } } = useUserApi();
    const { mutateAsync: add, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const submit = useCallback((data: UserAddFormModel) =>
        add(data).then(() => {
            queryClient.invalidateQueries([queryKey]);

            if (onSubmit) {
                onSubmit();
            }
        }), [add, queryClient, queryKey]);

    const {
        register,
        handleSubmit,
        watch,
        setValue,
        formState: { errors },
    } = useForm<UserAddFormModel>({
        defaultValues,
        resolver
    });

    const selectRole = useCallback((event: SelectChangeEvent<UserRoleEnum>) => {
        setValue("role", event.target.value as UserRoleEnum, {
            shouldValidate: true,
        });
    }, [setValue]);

    return {
        actions: {
            handleSubmit,
            submit,
            register,
            watch,
            selectRole
        },
        computed: {
            defaultValues,
            isSubmitting: status === "loading"
        },
        state: {
            errors
        }
    }
}