let isInBrowser = false;

let electronShell = {};

function bracketLink(link) {
    DotNet.invokeMethodAsync("Mytheme", "BracketLinkInterop", link);
}

function interceptClickEvent(e) {

    var tEvent = e || window.event;

    var element = tEvent.target || tEvent.srcElement;

    if (element.tagName === 'A' && element.href) {

        var link = element.href;

        if (link.indexOf('http://localhost') >= 0) {
            return true;
        } else {

            try {
                electronShell = require('electron').shell;
            } catch (e) {
                isInBrowser = true;
            }

            if (isInBrowser) {
                event.preventDefault();
                event.stopPropagation();
                window.open(event.target.href, '_blank');
            } else {
                event.preventDefault();
                event.stopPropagation();
                electronShell.openExternal(event.target.href);
                return false;
            }
        }
    }
}

//listen for link click events at the document level
if (document.addEventListener) {
    document.addEventListener('click', interceptClickEvent);
} else if (document.attachEvent) {
    document.attachEvent('onclick', interceptClickEvent);
}

////https://discuss.atom.io/t/how-to-intercept-electrons-request-and-return-a-customized-response/52312/3
////https://stackoverflow.com/questions/41068295/electron-prevent-cancel-page-navigation
////https://github.com/electron/electron/issues/1344




//let wc = require('electron').remote.getCurrentWebContents();

//wc.executeJavaScript('console.log(\'foo\')');

//wc.on('will-navigate', (e, u) => { linkIntercept(e, u)});
//wc.on('new-window', (e, u) => { linkIntercept(e, u) });



//function linkIntercept(event, url) {
//    if (url !== wc.getURL()) {

//        const shell = require('electron').shell;
//        const webview = document.querySelector('webview');

//        event.preventDefault();
//        webview.stop();
//        webview.getWebContents().stop();

//        shell.openExternal(url);

//        return false;
//    }
//}





