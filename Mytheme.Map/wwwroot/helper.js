window.getWindowSize = () => {
    return { height: window.innerHeight, width: window.innerWidth };
};

((window) => {
    let canvasContextCache = {};

    let getContext = (canvas) => {
        if (!canvasContextCache[canvas]) {
            canvasContextCache[canvas] = canvas.getContext('2d');
        }
        return canvasContextCache[canvas];
    };

    window.__blazorCanvasInterop = {

        getOffset: (canvas) => {
            var viewport = canvas.getBoundingClientRect();
            return { top: viewport.top, left: viewport.left};
        },

        getScrollOffset: (canvas) => {
            var viewport = canvas.getBoundingClientRect();
            return { top: window.scrollY, left: window.scrollX };
        },

        drawLine: (canvas, sX, sY, eX, eY) => {
            let context = getContext(canvas);

            context.lineJoin = 'round';
            context.lineWidth = 5;
            context.beginPath();
            context.moveTo(eX, eY);
            context.lineTo(sX, sY);
            context.closePath();
            context.stroke();
        },

        drawImage: (canvas, source) => {
            var img = new Image();
            img.src = source;

            img.onload = () => {
                let context = getContext(canvas);
                context.save();
                context.drawImage(img, 0, 0);
                context.restore();
            };
        },

        setContextPropertyValue: (canvas, propertyName, propertyValue) => {
            let context = getContext(canvas);

            context[propertyName] = propertyValue;
        }
    };
})(window);