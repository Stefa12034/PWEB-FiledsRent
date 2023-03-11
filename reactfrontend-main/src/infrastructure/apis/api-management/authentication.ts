import { LoginDTO } from "../client/models";
import { AuthorizationApi } from "../client/apis";

const loginMutationKey = "loginMutation";

export const useLoginApi = () => {
    const loginMutation = (loginDTO: LoginDTO) => new AuthorizationApi().apiAuthorizationLoginPost({ loginDTO }).then(response => {
        if (response.response?.token) {
            localStorage.setItem('token', response.response?.token ?? '');
        }
        
        return response;
    });

    return {
        loginMutation: {
            key: loginMutationKey,
            mutation: loginMutation
        }
    }
}