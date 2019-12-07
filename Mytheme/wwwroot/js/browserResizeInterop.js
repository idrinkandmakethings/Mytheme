window.browserResize = {
    getInnerHeight: function () {
        return window.innerHeight;
    },
    getInnerWidth: function () {
        return window.innerWidth;
    },
    register: function () {
        window.addEventListener("resize", browserResize.resized);
    },
    unregister: function () {
        window.removeEventListener("resize", browserResize.resized);
    },
    resized: function () {
        DotNet.invokeMethodAsync("Mytheme", 'OnBrowserResize').then(data => data);
    }
}