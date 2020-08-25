import { Configuration, Logger, LogLevel } from 'msal';

export const isSecureDomain = (): boolean => {
    const url = new URL(window.location.origin);
    return url.protocol.replace(':', '').trim() === 'https';
};

//Extend to string | number | symbol when there is a need
export const nameof = <T>(name: Extract<keyof T, string>): string => name;

export const throwIfNull = <T>(object: T, paramName: string) => {
    if (object === null || object === undefined)
        throw new Error(`Argument null or undefined exception: ${paramName}`);
    return object;
};

export const b2cAuthConfig = (
    clientId: string,
    authority: string
): Configuration => {
    return {
        auth: {
            clientId,
            authority,
            validateAuthority: false,
            navigateToLoginRequestUrl: false,
            redirectUri: window.location.origin,
            postLogoutRedirectUri: window.location.origin,
        },
        cache: {
            cacheLocation: 'localStorage',
            storeAuthStateInCookie: false,
        },
        system: {
            navigateFrameWait: 0,
            tokenRenewalOffsetSeconds: 500,
            loadFrameTimeout: 9000,
            logger: new Logger(
                (level, message) => {
                    switch (level) {
                        case LogLevel.Error:
                            console.error(message);
                            break;
                        case LogLevel.Warning:
                            console.warn(message);
                            break;
                        case LogLevel.Info:
                        case LogLevel.Verbose:
                        default:
                            console.log(message);
                            break;
                    }
                },
                { level: LogLevel.Verbose }
            ),
        },
    };
};

export const b2cScopeRequest = (
    loginScopes: string[],
    tenant: string,
    locale: string
) => {
    const scopeRequests = {
        login: {
            scopes: [],
            // eslint-disable-next-line camelcase
            extraQueryParameters: { ui_locales: locale },
            redirectUri: process.env.REACT_APP_AUTH_CALLBACK_URL as string,
        },
    };

    const applicationUri = process.env
        .REACT_APP_B2C_APPLICATION_ID_URI as string;

    scopeRequests.login.scopes = loginScopes.map((scope) => {
        return `https://${tenant}.onmicrosoft.com/api-mgt/${applicationUri}/${scope.trim()}` as never;
    });

    return scopeRequests;
};
