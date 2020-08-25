import { Given, When, Then } from "cucumber";
import { MyNotesApp } from "../Application/MyNotesApp";

Given("No one is logged in", async function () {
  await MyNotesApp.appInstance.getLoginPage().clearBrowserData();
});

Given("MyNotes is opened in browser", async function () {
  await MyNotesApp.appInstance.Init();
});

Given("I am on the login screen", async function () {
  await MyNotesApp.appInstance.GotoLoginPage();
  await MyNotesApp.appInstance.getLoginPage().AssertLoginPage();
});

Given("I start logging in", async function () {
  await MyNotesApp.appInstance.getLoginPage().start();
});

When(
  "I log in using account {string} and password {string}",
  { timeout: 2 * 5000 },
  async function (userName: string, password: string) {
    await MyNotesApp.appInstance.getLoginPage().login(userName, password);
  }
);

Then("I am succesfully logged in", async function () {
  await MyNotesApp.appInstance.AssertLoggedIn();
});
