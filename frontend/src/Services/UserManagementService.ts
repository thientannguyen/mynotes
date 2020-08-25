import axios from 'axios';
import { ServiceConfigFactory } from '../Infrastructure/ServiceConfigFactory';
import { User } from '../Models/User/User';
import NotificationService from '../Infrastructure/NotificationService';

export default class UserManagementService {
    private static notificationService = new NotificationService(
        process.env.REACT_APP_EASYFIT_NOTIFICATION_HUB_URL || ''
    );

    public static async createUser(
        userId: string,
        name: string,
        token: string
    ): Promise<void> {
        let user = new User(userId, name, []);

        const res = await axios.post(
            process.env.REACT_APP_EASYFIT_API_URL +
                '/UserManagement/CreateUser',
            JSON.stringify(user),
            ServiceConfigFactory.CreateAxiosConfigWithAuth(token)
        );
        if (res.status < 300) {
            return;
        }
        return Promise.reject('Failed to create user ' + res.statusText);
    }

    public static async getUser(
        userId: string,
        token: string
    ): Promise<User | undefined> {
        const res = await axios.get(
            process.env.REACT_APP_EASYFIT_API_URL +
                `/UserManagement/GetUser?userId=${userId}`,
            ServiceConfigFactory.CreateAxiosConfigWithAuth(token)
        );
        if (res.status === 200 && res.data !== null) {
            return res.data;
        } else if (res.status === 404 && res.statusText === 'Not Found') {
            return undefined;
        }
        return Promise.reject('Error occurred when getUser: ' + res.statusText);
    }

    public static async register(
        entityId: string,
        handler: (entityId: string) => Promise<void>
    ): Promise<void> {
        await this.notificationService.register(entityId, handler);
    }

    public static async unRegister(
        entityId: string,
        handler: (entityId: string) => Promise<void>
    ): Promise<void> {
        await this.notificationService.unRegister(entityId, handler);
    }
}
