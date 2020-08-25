import { When, Then } from 'cucumber';
import { MyNotesApp } from '../Application/MyNotesApp';
import {
    uniqueNamesGenerator,
    adjectives,
    colors,
    animals,
} from 'unique-names-generator';

When('I click on the signup button', async function () {
    await MyNotesApp.appInstance.getLoginPage().clicksOnTheSignUpLink();
});

When(
    'I input a random name as username and {string} as password',
    async function (password: string) {
        const randomName: string = uniqueNamesGenerator({
            dictionaries: [adjectives, colors, animals],
        });
        const userName = randomName + '@gmail.com';

        await MyNotesApp.appInstance
            .getLoginPage()
            .keyInUserNameAndPassword(userName, password);
    }
);

When('I click on create button', { timeout: 2 * 5000 }, async function () {
    await MyNotesApp.appInstance.getLoginPage().createAccount();
});

Then('I can see welcome text on the screen', async function () {
    await MyNotesApp.appInstance.getHomePage().assertHomePage();
});
