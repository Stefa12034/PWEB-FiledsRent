import { UserFileAddFormController, UserFileAddFormModel } from "./UserFileAddForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useUserFileApi } from "@infrastructure/apis/api-management";
import { useCallback } from "react";

const useInitUserFileAddForm = () => {
    const { formatMessage } = useIntl();

    const schema = yup.object().shape({
        file: yup.mixed()
            .required(formatMessage(
                { id: "globals.validations.requiredField" },
                {
                    fieldName: formatMessage({
                        id: "globals.file",
                    }),
                }))
    });

    const resolver = yupResolver(schema);

    return { defaultValues: {}, resolver };
}

export const useUserFileAddFormController = (onSubmit?: () => void): UserFileAddFormController => {
    const { defaultValues, resolver } = useInitUserFileAddForm();
    const { addUserFile: { mutation, key: mutationKey }, getUserFiles: { key: queryKey } } = useUserFileApi();
    const { mutateAsync: add, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const submit = useCallback((data: UserFileAddFormModel) =>
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
    } = useForm<UserFileAddFormModel>({
        defaultValues,
        resolver
    });

    const setFile = useCallback((file: File) => {
        setValue("file", file, {
            shouldValidate: true
        });
    }, [setValue]);

    return {
        actions: {
            handleSubmit,
            submit,
            register,
            watch,
            setFile
        },
        computed: {
            isSubmitting: status === "loading"
        },
        state: {
            errors
        }
    }
}