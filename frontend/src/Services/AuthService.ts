import {
    UserAgentApplication,
    Configuration,
    AuthResponse,
    AuthError,
    InteractionRequiredAuthError,
} from 'msal';
import {
    throwIfNull,
    b2cAuthConfig,
    b2cScopeRequest,
} from './AuthenticationUtils';
import UserManagementService from './UserManagementService';
import CookiesService from './CookiesService';
import { User } from '../Models/User/User';

export default class AuthService {
    public static adB2cInstance: AuthService = new AuthService();
    private readonly msalApp: UserAgentApplication;

    private readonly clientId: string = throwIfNull(
        process.env.REACT_APP_B2C_CLIENT_ID as string,
        'REACT_APP_B2C_CLIENT_ID missing from .env'
    );
    private readonly tenant: string = throwIfNull(
        process.env.REACT_APP_B2C_TENANT as string,
        'REACT_APP_B2C_TENANT missing from .env'
    );
    private readonly tenantId: string = throwIfNull(
        process.env.REACT_APP_B2C_TENANT_ID as string,
        'REACT_APP_B2C_TENANT_ID missing from .env'
    );
    private readonly signInPolicy: string = throwIfNull(
        process.env.REACT_APP_B2C_SIGN_IN_POLICY as string,
        'REACT_APP_B2C_SIGN_IN_POLICY missing from .env'
    );
    private readonly loginScopes: string = throwIfNull(
        process.env.REACT_APP_B2C_LOGIN_SCOPES as string,
        'REACT_APP_B2C_LOGIN_SCOPES missing from .env'
    );
    private readonly authority: string = `https://${this.tenant}.b2clogin.com/${this.tenantId}/${this.signInPolicy}`;

    getCurrentLocale = () => {
        return 'en';
    };

    private readonly scopeRequests = b2cScopeRequest(
        this.loginScopes.split(','),
        this.tenant,
        this.getCurrentLocale()
    );
    private readonly b2cConfiguration: Configuration = b2cAuthConfig(
        this.clientId,
        this.authority
    );

    private authenticationSuccessHandler: (returnUrl: string | null) => void;
    private authenticationErrorHandler: (
        error: string,
        errorDescription: string
    ) => void;

    private constructor() {
        this.authenticationSuccessHandler = (returnUrl: string | null) => {
            window.location.href = returnUrl !== null ? returnUrl : '/';
        };

        this.authenticationErrorHandler = (
            error: string,
            errorDescription: string
        ) => {};

        this.scopeRequests.login.extraQueryParameters = {
            // eslint-disable-next-line camelcase
            ui_locales: this.getCurrentLocale(),
        };

        this.msalApp = new UserAgentApplication(this.b2cConfiguration);

        this.authCallback = this.authCallback.bind(this);
        this.msalApp.handleRedirectCallback(this.authCallback);
    }

    private async authCallback(error: AuthError, response?: AuthResponse) {
        if (error != null) {
            this.authenticationErrorHandler(
                error.errorCode,
                error.errorMessage
            );
            return;
        }

        const userId = this.getObjectId();
        const name = this.getName();
        const token = await this.acquireAccessToken();
        const user = await UserManagementService.getUser(userId, token);
        if (user === undefined) {
            await UserManagementService.register(
                userId,
                this.onNotifiedUserCreated
            );
            await UserManagementService.createUser(userId, name, token);
        } else {
            CookiesService.serviceInstance.setUser(user);
            this.redirectUrl();
        }
    }

    private onNotifiedUserCreated = async (userId: string) => {
        console.log(`onNotifiedUserCreated user id: ${userId}`);
        const token = await this.acquireAccessToken();
        const user = await UserManagementService.getUser(userId, token);
        CookiesService.serviceInstance.setUser(user as User);
        await UserManagementService.unRegister(
            userId,
            this.onNotifiedUserCreated
        );
        this.redirectUrl();
    };

    private redirectUrl(): void {
        let returnurl = localStorage.getItem('returnUrl');
        if (returnurl !== null) {
            returnurl = decodeURIComponent(returnurl);
        }

        this.authenticationSuccessHandler(returnurl);
    }

    static getInstance() {
        if (!AuthService.adB2cInstance) {
            AuthService.adB2cInstance = new AuthService();
        }
        return AuthService.adB2cInstance;
    }

    public login(returnUrl: string): void {
        this.scopeRequests.login.extraQueryParameters = {
            // eslint-disable-next-line camelcase
            ui_locales: this.getCurrentLocale(),
        };
        localStorage.setItem('returnUrl', returnUrl);
        this.msalApp.loginRedirect(this.scopeRequests.login);
    }

    public logout(): void {
        this.msalApp.logout();
        localStorage.clear();
        sessionStorage.clear();
        CookiesService.serviceInstance.clearCookies();
    }

    private async getAccessToken() {
        this.scopeRequests.login.extraQueryParameters = {
            // eslint-disable-next-line camelcase
            ui_locales: this.getCurrentLocale(),
        };

        return await this.msalApp
            .acquireTokenSilent(this.scopeRequests.login)
            .catch((error) => {
                if (error instanceof InteractionRequiredAuthError) {
                    return this.msalApp.acquireTokenRedirect(
                        this.scopeRequests.login
                    );
                } else {
                    this.authenticationErrorHandler(
                        error.errorCode,
                        error.errorMessage
                    );
                }

                return undefined;
            });
    }

    public getUserInfo() {
        if (this.msalApp.getAccount() === null) return null;
        return this.msalApp.getAccount().idToken;
    }

    public async acquireAccessToken(): Promise<string> {
        const token = await this.getAccessToken();
        if (token) {
            return token.accessToken;
        } else {
            this.authenticationErrorHandler(
                'Authentication required',
                'Session expired'
            );
        }
        return '';
    }

    public isUserNew() {
        if (this.msalApp.getAccount() === null) return false;
        let token = this.msalApp.getAccount().idToken;
        console.log('Is new user:' + token['newUser']);

        return token['newUser'] !== undefined;
    }

    public getIsAuthenticated(): boolean {
        return this.msalApp.getAccount() !== null;
    }

    public getObjectId(): string {
        let token = this.msalApp.getAccount().idToken;
        return token['oid'];
    }

    public getName(): string {
        let token = this.msalApp.getAccount().idToken;
        return token['name'];
    }
}
