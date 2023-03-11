import { useIntl } from "react-intl";
import { useInterceptor } from "@infrastructure/hooks/useInterceptor";
import { toast, ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import { ErrorResponse } from "@application/models/ErrorResponse";
import { is } from "@infrastructure/utils/typeUtils";
import { useTokenHasExpired } from "@infrastructure/hooks/useOwnUser";
import { ErrorCodes } from "@infrastructure/apis/client";

const getTranslationIdForKey = (code?: ErrorCodes) => {
    switch (code) {
        case ErrorCodes.CannotAdd:
            return { id: "notifications.errors.cannotAdd" };

        case ErrorCodes.CannotDelete:
            return { id: "notifications.errors.cannotDelete" };

        case ErrorCodes.CannotUpdate:
            return { id: "notifications.errors.cannotUpdate" };

        case ErrorCodes.EntityNotFound:
            return { id: "notifications.errors.entityNotFound" };

        case ErrorCodes.MailSendFailed:
            return { id: "notifications.errors.mailSendFailed" };

        case ErrorCodes.PhysicalFileNotFound:
            return { id: "notifications.errors.physicalFileNotFound" };

        case ErrorCodes.TechnicalError:
            return { id: "notifications.errors.technicalError" };

        case ErrorCodes.UserAlreadyExists:
            return { id: "notifications.errors.userAlreadyExists" };

        case ErrorCodes.WrongPassword:
            return { id: "notifications.errors.wrongPassword" };

        default:
            return { id: "notifications.errors.unknownHappened" };
    }
};

export const ToastNotifier = () => {
    const { formatMessage } = useIntl();
    const tokenHasExpired = useTokenHasExpired();

    useInterceptor({
        async onResponse(response: Response) {
            if (response.status === 401 && tokenHasExpired.loggedIn && tokenHasExpired.hasExpired) {
                toast.error(formatMessage({ id: "notifications.errors.sessionExpired" }));
            } else if (response.status === 500) {
                toast.error(formatMessage({ id: "notifications.errors.unknownHappened" }));
            } else if (!response.ok && response.headers.has("content-type") && response.headers.get("content-type")?.includes("application/json")) {
                const cloned = response.clone();
                const error = await cloned.json();

                if (error && is<ErrorResponse>(error)) {
                    toast.error(formatMessage(
                        { id: "notifications.errors.errorMessage" },
                        {
                            code: formatMessage(getTranslationIdForKey(error.errorMessage.code))
                        }
                    ));
                }
            }

            return response;
        },
        onResponseError(error: any) {
            if (error.message === 'Failed to fetch') {
                toast.error(formatMessage({ id: "notifications.errors.networkError" }));
            }

            return error;
        }
    });

    return <ToastContainer
        position="top-left"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
        limit={1}
    />
}