import { browser, ExpectedConditions } from 'protractor';
import { assert } from 'chai';
import '../Protractor/extensions';
import { MyNotesLogin } from '../Steps/Pages/MyNotesLogin';
import { HomePage } from '../Steps/Pages/HomePage';

export class App {
    public baseUrl = browser.params.env.baseUrl;
    public apiUrl = browser.params.env.apiUrl;
    private authAppId = browser.params.env.authAppId;

    private login = new MyNotesLogin();
    private homePage = new HomePage();

    public async SaveChangeRoute(
        subUrl: string,
        locale?: string
    ): Promise<void> {
        var path = this.BuildFullUrlPath(subUrl);
        await browser.driver.get(path);
        await browser.waitForReact();
        await this.assertIsOnPage(subUrl);
    }

    public async assertIsOnPage(pageSubUrl: string): Promise<void> {
        await browser.wait(ExpectedConditions.urlContains(pageSubUrl), 6000);
    }

    public getLoginPage() {
        return this.login;
    }

    public getHomePage() {
        return this.homePage;
    }

    public async AssertLoggedIn() {
        await browser.wait(async () => {
            return await this.IsloggedIn();
        }, 5000);
    }

    private async IsloggedIn(): Promise<boolean> {
        console.log(this.authAppId);
        return await browser.executeScript(
            `return window.localStorage.getItem('msal.${this.authAppId}.idtoken');`
        );
    }

    public async GotoLoginPage(locale?: string): Promise<MyNotesLogin> {
        await this.SaveChangeRoute('login', locale);
        return this.getLoginPage();
    }

    public async GotoHomePage(): Promise<void> {
        await this.SaveChangeRoute('');
    }

    public async Init(): Promise<void> {
        await browser.waitForAngularEnabled(false);
        await this.GotoHomePage();
    }

    public async AssertBrowserTitle(expectedTitle: string): Promise<void> {
        var actualTitle = await browser.getTitle();
        assert.equal(actualTitle, expectedTitle);
    }

    private BuildFullUrlPath(subUrl: string): string {
        return this.baseUrl + '/' + subUrl;
    }
}
