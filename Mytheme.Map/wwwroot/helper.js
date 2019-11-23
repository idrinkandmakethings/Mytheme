var mapImage;

let ImgIsLoaded = false;


window.getWindowSize = () => {
    return { height: window.innerHeight, width: window.innerWidth };
};

((window) => {
    let canvasContextCache = {};
    let canvasXforms = {};

    let getContext = (canvas) => {
        if (!canvasContextCache[canvas.id]) {
            canvasContextCache[canvas.id] = canvas.getContext('2d');
            canvasXforms[canvas.id] = {
                dragStart: null,
                dragged: false,
                lastX: canvas.width / 2,
                lastY: canvas.width / 2
            };
            trackTransforms(canvasContextCache[canvas.id]);
        }
        return canvasContextCache[canvas.id];
    };

    window.__blazorCanvasInterop = {

        loadCanvases: (canvasList) => {
            canvasList.forEach(e => getContext(e));
        },

        redraw: (canvas, drawMap) => {
            let ctx = getContext(canvas);

           // var p1 = ctx.transformedPoint(0, 0);
          //  var p2 = ctx.transformedPoint(canvas.width, canvas.height);
           // ctx.clearRect(p1.x, p1.y, p2.x - p1.x, p2.y - p1.y);

            ctx.save();
           
            ctx.setTransform(1, 0, 0, 1, 0, 0);
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.restore();

            if (drawMap) {
                ctx.drawImage(mapImage, 0, 0);
            }
        },


        getOffset: (canvas) => {
            var viewport = canvas.getBoundingClientRect();
            return { top: viewport.top, left: viewport.left};
        },

        getScrollOffset: () => {
            return { top: window.scrollY, left: window.scrollX };
        },

        panStart: (x, y) => {
            for (var key in canvasContextCache) {
                let ctx = canvasContextCache[key];
                let xform = canvasXforms[key];
                xform.lastX = x;
                xform.lastY = y;
                xform.dragStart = ctx.transformedPoint(xform.lastX, xform.lastY);
                xform.dragged = false;
            }
        },

        pan: (x, y) => {
            for (var key in canvasContextCache) {
                let ctx = canvasContextCache[key];
                let xform = canvasXforms[key];
                xform.lastX = x;
                xform.lastY = y;
                xform.dragged = true;
                if (xform.dragStart) {
                    var pt = ctx.transformedPoint(xform.lastX, xform.lastY);
                    ctx.translate(pt.x - xform.dragStart.x, pt.y - xform.dragStart.y);
                    this.__blazorCanvasInterop.redraw(ctx.canvas, ctx.canvas.id === "mapLayer");
                }
            }
        },

        panStop: (x, y) => {
            for (var key in canvasContextCache) {
                let ctx = canvasContextCache[key];
                let xform = canvasXforms[key];
                xform.dragStart = null;
            }
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

        drawImage: (canvas, val) => {

            return new Promise(done => {
                var image = new Image();

                image.onload = () => {
                    let context = getContext(canvas);
                    context.drawImage(image, 0, 0);
                    done(true);
                };

                image.src = val;
            });

        },

        preLoad_Image: function (image) {
            return new Promise(resolve => {


                if (!ImgIsLoaded) {
                    mapImage = new Image();

                    mapImage.onload = () => {
                        ImgIsLoaded = true;
                        resolve("promise resolved, loading done");
                    };
                    mapImage.src = image;
                }
                else {

                    resolve("image already loaded");
                }
            });
        },

        setContextPropertyValue: (canvas, propertyName, propertyValue) => {
            let context = getContext(canvas);

            context[propertyName] = propertyValue;
        }
    };
})(window);

// Adds ctx.getTransform() - returns an SVGMatrix
// Adds ctx.transformedPoint(x,y) - returns an SVGPoint
function trackTransforms(ctx) {
    var svg = document.createElementNS("http://www.w3.org/2000/svg", 'svg');
    var xform = svg.createSVGMatrix();
    ctx.getTransform = function () { return xform; };

    var savedTransforms = [];
    var save = ctx.save;
    ctx.save = function () {
        savedTransforms.push(xform.translate(0, 0));
        return save.call(ctx);
    };

    var restore = ctx.restore;
    ctx.restore = function () {
        xform = savedTransforms.pop();
        return restore.call(ctx);
    };

    var scale = ctx.scale;
    ctx.scale = function (sx, sy) {
        xform = xform.scaleNonUniform(sx, sy);
        return scale.call(ctx, sx, sy);
    };

    var rotate = ctx.rotate;
    ctx.rotate = function (radians) {
        xform = xform.rotate(radians * 180 / Math.PI);
        return rotate.call(ctx, radians);
    };

    var translate = ctx.translate;
    ctx.translate = function (dx, dy) {
        xform = xform.translate(dx, dy);
        return translate.call(ctx, dx, dy);
    };

    var transform = ctx.transform;
    ctx.transform = function (a, b, c, d, e, f) {
        var m2 = svg.createSVGMatrix();
        m2.a = a; m2.b = b; m2.c = c; m2.d = d; m2.e = e; m2.f = f;
        xform = xform.multiply(m2);
        return transform.call(ctx, a, b, c, d, e, f);
    };

    var setTransform = ctx.setTransform;
    ctx.setTransform = function (a, b, c, d, e, f) {
        xform.a = a;
        xform.b = b;
        xform.c = c;
        xform.d = d;
        xform.e = e;
        xform.f = f;
        return setTransform.call(ctx, a, b, c, d, e, f);
    };

    var pt = svg.createSVGPoint();
    ctx.transformedPoint = function (x, y) {
        pt.x = x; pt.y = y;
        return pt.matrixTransform(xform.inverse());
    }
}