const cucumberSetup = require("./mynotes.protractor.testsetup.js");

const baseUrl = "http://localhost:3000";
const apiUrl = "http://localhost:35401/api";

const executeInParallel = false;

const generalConfig = {
  beforeLaunch: cucumberSetup.beforeLaunch(),
  //afterLaunch: cucumberSetup.afterLaunch(apiUrl),

  seleniumAddress: cucumberSetup.seleniumAddress, // This is targeting your local running instance of the selenium webdriver
  ignoreUncaughtExceptions: true,
  framework: cucumberSetup.framework, //We need this line to use the cucumber framework
  frameworkPath: cucumberSetup.frameworkPath,
  cucumberOpts: cucumberSetup.cucumberOpts,
  plugins: cucumberSetup.plugins,

  params: {
    env: {
      baseUrl: baseUrl,
      apiUrl: apiUrl,
      defaultLocale: "en",
      authAppId: "2276b650-72c4-424d-bc75-c843d8a58085",
      // add auth when needed
    },
  },
};

const parallelConfig = {
  multiCapabilities: cucumberSetup.multiCapabilities,
};

const sequentialConfig = {
  specs: ["../Features/*.feature"],
  capabilities: {
    browserName: "chrome",
    chromeOptions: {
      args: cucumberSetup.browserOptions.filter((opt) => opt !== "--headless"),
    },
  },
};

exports.config = Object.assign(
  generalConfig,
  executeInParallel ? parallelConfig : sequentialConfig
);
