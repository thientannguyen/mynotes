import { App } from "./App";

export class MyNotesApp {
  public static appInstance: App;
  public static async createApp() {
    this.appInstance = new App();
  }
}
