const standardBrowserOptions = [
  "--headless",
  "--window-size=1920,1080",
  "--no-sandbox",
  "--disable-gpu",
  "--enable-features=NetworkService",
  "--incognito",
];

const multiCapabilities = [
  {
    browserName: "chrome",
    chromeOptions: {
      args: standardBrowserOptions,
    },
    specs: ["../Features/*.feature"],
  },
  // add another bucket for parallel feature testing
];

const cucumberOpts = {
  require: "../build/**/*.js", // This is where we'll be writing our actual tests
  format: "json:Protractor/result.json",
  tags: "~@ignore",
};

const plugins = [
  {
    package: "protractor-multiple-cucumber-html-reporter-plugin",
    options: {
      automaticallyGenerateReport: true,
      removeExistingJsonReportFile: true,
      saveCollectedJSON: true,
      displayDuration: true,
      openReportInBrowser: true,
    },
  },
  { package: "protractor-react-selector" },
  {
    package: "protractor-testability-plugin",
    customFrameworkTestability: {
      setLocation: function (route) {
        Backbone.history.navigate(route, true);
      },
    },
  },
];

function deleteResultFiles() {
  const fs = require("fs");
  const glob = require("glob");

  console.log("\n");

  glob.sync("Protractor/result*.json").forEach((file) => {
    console.log("Delete result file " + file);
    fs.unlinkSync(file);
  });
  glob.sync("Protractor/json-output-folder/*.json").forEach((file) => {
    console.log("Delete result file " + file);
    fs.unlinkSync(file);
  });
  glob.sync("Protractor/report/*.*").forEach((file) => {
    console.log("Delete result file " + file);
    fs.unlinkSync(file);
  });

  console.log("\n");
}

async function configureNonAngularApp() {
  /**
   * If you are testing against a non-angular site - set ignoreSynchronization setting to true
   *
   * If true, Protractor will not attempt to synchronize with the page before
   * performing actions. This can be harmful because Protractor will not wait
   * until $timeouts and $http calls have been processed, which can cause
   * tests to become flaky. This should be used only when necessary, such as
   * when a page continuously polls an API using $timeout.
   *
   * @type {boolean}
   */

  await browser.waitForAngularEnabled(false);
}

module.exports = {
  plugins: plugins,
  cucumberOpts: cucumberOpts,
  multiCapabilities: multiCapabilities,
  framework: "custom",
  frameworkPath: require.resolve("protractor-cucumber-framework"),
  seleniumAddress: "http://localhost:4444/wd/hub",
  browserOptions: standardBrowserOptions,

  /* Do stuff on global --BEFORE ANY TESTS IS EXECUTED-- */
  beforeLaunch() {
    return () => {
      deleteResultFiles();
    };
  },

  onPrepare: function () {
    return async () => {
      await configureNonAngularApp();
    };
  },

  //afterLaunch(apiUrl) {} /* Do stuff on global --AFTER ALL TESTS ARE EXECUTED-- */
};
