function interceptClickEvent(e) {

    var tEvent = e || window.event;

    var element = tEvent.target || tEvent.srcElement;

    if (element.tagName === 'A' && element.href) {

        var link = element.href;

        if (link.indexOf('bracketlink') >= 0) {
            console.log("intercept!");
            event.preventDefault();
            DotNet.invokeMethodAsync("Mytheme", "BracketLinkInterop", link, element.innerText);
        }
        else
        {
            return true;
        }
    }
}


function AddIconToMap() {
    
}

//listen for link click events at the document level
if (document.addEventListener) {
    document.addEventListener('click', interceptClickEvent);
} else if (document.attachEvent) {
    document.attachEvent('onclick', interceptClickEvent);
}


