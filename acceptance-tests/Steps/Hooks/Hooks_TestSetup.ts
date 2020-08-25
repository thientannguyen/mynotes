import { BeforeAll, After, HookScenarioResult, Status } from 'cucumber';
import { MyNotesApp } from '../../Application/MyNotesApp';
import { browser } from 'protractor';
import CommonUtilities from '../../CommonUtilities';

BeforeAll({ timeout: 15000 }, async function () {
    await MyNotesApp.createApp();
});

After({ timeout: 15000 }, async function (result: HookScenarioResult) {
    console.log('END OF SCENARIO');
    if (result.result.status == Status.FAILED) {
        //capture screenshot after each scenario
        let screenshot = await browser.driver.takeScreenshot();
        this.attach(screenshot, 'image/png');
    }
    await CommonUtilities.wait();

    await MyNotesApp.appInstance.getHomePage().clickLogout();

    await CommonUtilities.wait();
});
