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






