import { useAppSelector } from "@application/store";
import { ApiUserFileAddPostRequest, ApiUserFileGetPageGetRequest, UserFileApi } from "../client";
import { getAuthenticationConfiguration } from "@infrastructure/utils/userUtils";

const getUserFilesQueryKey = "getUserFilesQuery";
const downloadUserFileQueryKey = "downloadUserFileQuery";
const addUserFileMutationKey = "addUserFileMutation";

export const useUserFileApi = () => {
    const { token } = useAppSelector(x => x.profileReducer);

    const getUserFiles = (page: ApiUserFileGetPageGetRequest) => new UserFileApi(getAuthenticationConfiguration(token)).apiUserFileGetPageGet(page);
    const downloadUserFile = (id: string) => new UserFileApi(getAuthenticationConfiguration(token)).apiUserFileDownloadIdGet({ id });
    const addUserFile = (userFile: ApiUserFileAddPostRequest) => new UserFileApi(getAuthenticationConfiguration(token)).apiUserFileAddPost(userFile);

    return {
        getUserFiles: {
            key: getUserFilesQueryKey,
            query: getUserFiles
        },
        downloadUserFile: {
            key: downloadUserFileQueryKey,
            query: downloadUserFile
        },
        addUserFile: {
            key: addUserFileMutationKey,
            mutation: addUserFile
        }
    }
}