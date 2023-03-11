import { useAppSelector } from "@application/store";
import { ApiUserGetPageGetRequest, UserAddDTO, UserApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";

const getUsersQueryKey = "getUsersQuery";
const getUserQueryKey = "getUserQuery";
const addUserMutationKey = "addUserMutation";
const deleteUserMutationKey = "deleteUserMutation";

export const useUserApi = () => {
    const { token } = useAppSelector(x => x.profileReducer);

    const getUsers = (page: ApiUserGetPageGetRequest) => new UserApi(getAuthenticationConfiguration(token)).apiUserGetPageGet(page);
    const getUser = (id: string) => new UserApi(getAuthenticationConfiguration(token)).apiUserGetByIdIdGet({ id });
    const addUser = (user: UserAddDTO) => new UserApi(getAuthenticationConfiguration(token)).apiUserAddPost({ userAddDTO: user });
    const deleteUser = (id: string) => new UserApi(getAuthenticationConfiguration(token)).apiUserDeleteIdDelete({ id });

    return {
        getUsers: {
            key: getUsersQueryKey,
            query: getUsers
        },
        getUser: {
            key: getUserQueryKey,
            query: getUser
        },
        addUser: {
            key: addUserMutationKey,
            mutation: addUser
        },
        deleteUser: {
            key: deleteUserMutationKey,
            mutation: deleteUser
        }
    }
}