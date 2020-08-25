const findChromeVersion = require("find-chrome-version");

async function process() {
  const chromeVersion = await findChromeVersion();
    // console log will be used as result. Do not delete this log!
  console.log(`${chromeVersion}`);
}

(async () => {
  try {
    await process();
  }
  catch(e) {
    await new Promise(resolve => setTimeout(resolve, 2000));
    await process();
  }
})();