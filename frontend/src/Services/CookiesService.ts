import { Cookies } from 'react-cookie';
import { User } from '../Models/User/User';

export default class CookiesService {
    public static serviceInstance: CookiesService = new CookiesService();
    private cookies: Cookies;

    constructor() {
        this.cookies = new Cookies();
    }

    public getUserId(): string {
        var result = this.cookies.get('UserId');
        return result !== undefined ? result : '';
    }

    public setUser(user: User) {
        this.cookies.set('UserId', user.id);
    }

    public clearCookies() {
        const allCookies = ['UserId'];

        allCookies.forEach((value) => {
            if (this.cookies.get(value) !== undefined)
                this.cookies.remove(value);
        });
    }
}
