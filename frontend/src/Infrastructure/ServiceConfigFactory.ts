import { AxiosRequestConfig } from 'axios';

export class ServiceConfigFactory {
    public static CreateAxiosConfigWithAuth(token: string): AxiosRequestConfig {
        return {
            validateStatus: (_) => true,
            timeout: 10000,
            headers: {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + token,
            },
        };
    }

    public static CreateAxiosConfigWithoutAuth(): AxiosRequestConfig {
        return {
            validateStatus: (_) => true,
            timeout: 10000,
            headers: {
                'Content-Type': 'application/json',
            },
        };
    }
}
