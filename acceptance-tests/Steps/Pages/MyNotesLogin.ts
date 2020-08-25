import { browser } from 'protractor';
import { element, By } from 'protractor';
import '../../Protractor/extensions';

export class MyNotesLogin {
    public async start() {
        await element(By.id('btn-start')).safeClick();
    }

    public async login(userName: string, password: string) {
        await element(By.id('logonIdentifier')).safeSendKeys(userName);
        await element(By.id('password')).safeSendKeys(password);
        await this.timeout(3000);
        await element(By.id('next')).safeClick();
        await this.timeout(3000);
    }

    private timeout(ms: number) {
        return new Promise((resolve) => setTimeout(resolve, ms));
    }

    public async clearBrowserData(): Promise<any> {
        await browser.manage().deleteAllCookies();
        await browser.executeScript(
            'try{window.sessionStorage.clear(); window.localStorage.clear();}catch(err){}'
        );
    }

    public async AssertLoginPage() {
        await element(By.id('btn-start')).waitToBeVisible();
    }

    public async assertContinueButtonIsVisible() {
        await element(By.id('btn-start')).waitTextToBePresent('CONTINUE');
    }

    public async clicksOnTheSignUpLink() {
        element(By.id('createAccount')).safeClick();
        await this.timeout(3000);
    }

    public async logout() {
        element(By.id('createAccount')).safeClick();
        await this.timeout(3000);
    }

    public keyInUserNameAndPassword(username: string, password: string) {
        element(By.id('email')).safeSendKeys(username);
        element(By.id('newPassword')).safeSendKeys(password);
        element(By.id('reenterPassword')).safeSendKeys(password);
        element(By.id('displayName')).safeSendKeys(username);
    }

    public async createAccount() {
        element(By.id('continue')).safeClick();
        await this.timeout(8000);
    }
}
