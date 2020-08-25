export default class CommonUtilities {
    private static durationMillis = 3000;

    public static wait() {
        return new Promise((resolve) =>
            setTimeout(resolve, CommonUtilities.durationMillis)
        );
    }
}
