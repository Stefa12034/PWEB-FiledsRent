import { Configuration } from "@infrastructure/apis/client";
import { isNull } from "lodash";

export const getAuthenticationConfiguration = (token: string | null) => new Configuration(!isNull(token) ? { apiKey: `Bearer ${localStorage.getItem('token')}` } : {});