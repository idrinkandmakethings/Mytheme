using Microsoft.AspNetCore.Components;

namespace Mytheme.Map.Utility
{
    public class SvgHelper
    {

        private string pencilD = "M6 0l-1 1 2 2 1-1-2-2zm-2 2l-4 4v2h2l4-4-2-2z";
        private string compassD = "M4 0c-2.2 0-4 1.8-4 4s1.8 4 4 4 4-1.8 4-4-1.8-4-4-4zm0 1c1.66 0 3 1.34 3 3s-1.34 3-3 3-3-1.34-3-3 1.34-3 3-3zm2 1l-3 1-1 3 3-1 1-3zm-2 1.5c.28 0 .5.22.5.5s-.22.5-.5.5-.5-.22-.5-.5.22-.5.5-.5z";
        private string markerD = "M3 0c-1.66 0-3 1.34-3 3 0 2 3 5 3 5s3-3 3-5c0-1.66-1.34-3-3-3zm0 1c1.11 0 2 .9 2 2 0 1.11-.89 2-2 2-1.1 0-2-.89-2-2 0-1.1.9-2 2-2z";
        private string zoomInD = "M3.5 0c-1.93 0-3.5 1.57-3.5 3.5s1.57 3.5 3.5 3.5c.61 0 1.19-.16 1.69-.44a1 1 0 0 0 .09.13l1 1.03a1.02 1.02 0 1 0 1.44-1.44l-1.03-1a1 1 0 0 0-.13-.09c.27-.5.44-1.08.44-1.69 0-1.93-1.57-3.5-3.5-3.5zm0 1c1.39 0 2.5 1.11 2.5 2.5 0 .59-.2 1.14-.53 1.56-.01.01-.02.02-.03.03a1 1 0 0 0-.06.03 1 1 0 0 0-.25.28c-.44.37-1.01.59-1.63.59-1.39 0-2.5-1.11-2.5-2.5s1.11-2.5 2.5-2.5zm-.5 1v1h-1v1h1v1h1v-1h1v-1h-1v-1h-1z";
        private string zoomOutD = "M3.5 0c-1.93 0-3.5 1.57-3.5 3.5s1.57 3.5 3.5 3.5c.61 0 1.19-.16 1.69-.44a1 1 0 0 0 .09.13l1 1.03a1.02 1.02 0 1 0 1.44-1.44l-1.03-1a1 1 0 0 0-.13-.09c.27-.5.44-1.08.44-1.69 0-1.93-1.57-3.5-3.5-3.5zm0 1c1.39 0 2.5 1.11 2.5 2.5 0 .59-.2 1.14-.53 1.56-.01.01-.02.02-.03.03a1 1 0 0 0-.06.03 1 1 0 0 0-.25.28c-.44.37-1.01.59-1.63.59-1.39 0-2.5-1.11-2.5-2.5s1.11-2.5 2.5-2.5zm-1.5 2v1h3v-1h-3z";


        public string FillColor { get; set; }

        public int Size { get; set; }

        public SvgHelper(string fillColor, int size)
        {
            FillColor = fillColor;
            Size = size;
        }

        public MarkupString GetDraw()
        {
            return GetSvgString(pencilD);
        }

        public MarkupString GetPan()
        {
            return GetSvgString(compassD);
        }

        public MarkupString GetMarker()
        {
            return GetSvgString(markerD, "transform=\"translate(1)\"");
        }

        public MarkupString GetZoomIn()
        {
            return GetSvgString(zoomInD);
        }

        public MarkupString GetZoomOut()
        {
            return GetSvgString(zoomOutD);
        }

        private MarkupString GetSvgString(string dValue, string transform = "")
        {
            return  new MarkupString($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{Size}\" height=\"{Size}\" viewBox=\"0 0 8 8\">\r\n  <path style=\"fill: {FillColor}\" d=\"{dValue}\" {transform} />\r\n</svg>");
        }
    }
}
