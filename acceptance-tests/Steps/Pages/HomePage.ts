import { element, By } from 'protractor';
import '../../Protractor/extensions';

export class HomePage {
    public async assertHomePage() {
        await element(By.id('txt-title')).waitToBeVisible();
    }

    public async clickLogout() {
        await element(By.id('btn-logout')).safeClick();
    }
}
