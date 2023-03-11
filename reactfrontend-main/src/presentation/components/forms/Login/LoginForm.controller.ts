import { LoginFormController, LoginFormModel } from "./LoginForm.types";
import { yupResolver } from "@hookform/resolvers/yup";
import { useIntl } from "react-intl";
import * as yup from "yup";
import { isUndefined } from "lodash";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useLoginApi } from "@infrastructure/apis/api-management";
import { useCallback } from "react";
import { useAppRouter } from "@infrastructure/hooks/useAppRouter";
import { useDispatch } from "react-redux";
import { setToken } from "@application/state-slices";
import { toast } from "react-toastify";

const getDefaultValues = (initialData?: { email: string }) => {
    const defaultValues = {
        email: "",
        password: ""
    };

    if (!isUndefined(initialData)) {
        return {
            ...defaultValues,
            ...initialData,
        };
    }

    return defaultValues;
};

const useInitLoginForm = () => {
    const { formatMessage } = useIntl();
    const defaultValues = getDefaultValues();

    const schema = yup.object().shape({
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
                }))
            .default(defaultValues.password),
    });

    const resolver = yupResolver(schema);

    return { defaultValues, resolver };
}

export const useLoginFormController = (): LoginFormController => {
    const { formatMessage } = useIntl();
    const { defaultValues, resolver } = useInitLoginForm();
    const { redirectToHome } = useAppRouter();
    const { loginMutation: { mutation, key: mutationKey } } = useLoginApi();
    const { mutateAsync: login, status } = useMutation([mutationKey], mutation);
    const queryClient = useQueryClient();
    const dispatch = useDispatch();
    const submit = useCallback((data: LoginFormModel) =>
        login(data).then((result) => {
            dispatch(setToken(result.response?.token ?? ''));
            toast(formatMessage({ id: "notifications.messages.authenticationSuccess" }))
            redirectToHome();
        }), [login, queryClient, redirectToHome, dispatch]);

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<LoginFormModel>({
        defaultValues,
        resolver,
    });

    return {
        actions: {
            handleSubmit,
            submit,
            register
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