function requireConfig(textPath) {
    define('knockout', [], function () { return ko; });
    define('ko', [], function () { return ko; });
    require.config({
        paths: {
            "text": textPath
        },
        urlArgs: "cacheBust=" + SITE_INSTANCE   //from _layout.cshtml
    });
}