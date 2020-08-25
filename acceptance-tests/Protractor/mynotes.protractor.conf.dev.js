const cucumberSetup = require('./mynotes.protractor.testsetup.js');

const baseUrl = 'https://mynotes-dev.azurewebsites.net';
const apiUrl = 'https://mynotes-api-dev.azurewebsites.net/api';

exports.config = {
    beforeLaunch: cucumberSetup.beforeLaunch(apiUrl),
    //afterLaunch: cucumberSetup.afterLaunch(apiUrl),
    onPrepare: cucumberSetup.onPrepare(),

    seleniumAddress: cucumberSetup.seleniumAddress, // This is targeting your local running instance of the selenium webdriver

    ignoreUncaughtExceptions: true,
    multiCapabilities: cucumberSetup.multiCapabilities,
    framework: cucumberSetup.framework, //We need this line to use the cucumber framework
    frameworkPath: cucumberSetup.frameworkPath,
    cucumberOpts: cucumberSetup.cucumberOpts,
    plugins: cucumberSetup.plugins,

    params: {
        env: {
            baseUrl: baseUrl,
            apiUrl: apiUrl,
            defaultLocale: 'en',
            authAppId: 'e0bc21b1-81fd-4f54-b2b1-386bc1b734ad',
        },
    },
};
