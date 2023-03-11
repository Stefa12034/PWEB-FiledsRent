import { useAppSelector } from "@application/store"
import { useUserApi } from "@infrastructure/apis/api-management";
import { UserRoleEnum } from "@infrastructure/apis/client";
import { useQuery } from "@tanstack/react-query";
import { isNull, isUndefined } from "lodash";

export const useOwnUser = () => {
    const { userId } = useAppSelector(x => x.profileReducer);
    const { getUser: { key: queryKey, query } } = useUserApi();
    const { data } = useQuery([queryKey, userId], () => {
        if (isNull(userId)) {
            return null;
        }

        return query(userId);
    }, {
        refetchInterval: Infinity,
        refetchOnWindowFocus: false
    });

    return data?.response;
}

export const useOwnUserHasRole = (role: UserRoleEnum) => {
    const ownUser = useOwnUser();

    if (isUndefined(ownUser)) {
        return;
    }

    return ownUser.role === role;
}

export const useTokenHasExpired = () => {
    const { loggedIn, exp } = useAppSelector(x => x.profileReducer);
    const now = Date.now() / 1000;

    return {
        loggedIn,
        hasExpired: !exp || exp < now
    };
}