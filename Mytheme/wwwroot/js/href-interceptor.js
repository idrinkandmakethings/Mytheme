let isInBrowser = false;

let electronShell = {};

function interceptClickEvent(e) {

    try {
        electronShell = require('electron').shell;
    } catch (e) {
        isInBrowser = true;
    }

    var tEvent = e || window.event;

    var element = tEvent.target || tEvent.srcElement;

    if (element.tagName === 'A' && element.href) {

        var link = element.href;

        if (link.indexOf('bracketlink') >= 0) {
            console.log("intercept!");
            event.preventDefault();
            event.stopPropagation();
            DotNet.invokeMethodAsync("Mytheme", "BracketLinkInterop", link, element.innerText);

            if (!isInBrowser) {
                setTimeout(function() {
                        var path = e.target.href;
                        ipcRenderer.sendToHost('element-clicked', path);
                    },
                    100);
                return false;
            }
        }
        else if (link.indexOf('http://localhost') >= 0) {
            return true;
        } else {
            if (isInBrowser) {
                event.preventDefault();
                window.open(event.target.href, '_blank');
            } else {
                event.preventDefault();
                electronShell.openExternal(event.target.href);
            }
        }
    }
}

//https://discuss.atom.io/t/how-to-intercept-electrons-request-and-return-a-customized-response/52312/3
//https://stackoverflow.com/questions/41068295/electron-prevent-cancel-page-navigation
try {
    const session  = require('electron');
    const ses = session.session;

    ses.webRequest.onBeforeRequest((details, callback) => {
        if (details.url.indexOf('bracketlink') >= 0) {
            callback({ cancel: true });
        } else {
            callback({});
        }
    });

} catch (e) {
    // not in electron instance...
}


//listen for link click events at the document level
if (document.addEventListener) {
    document.addEventListener('click', interceptClickEvent);
} else if (document.attachEvent) {
    document.attachEvent('onclick', interceptClickEvent);
}


