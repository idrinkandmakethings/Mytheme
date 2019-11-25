using Microsoft.AspNetCore.Components;

namespace Mytheme.Services
{
    public class SvgHelperService
    {
        private const string menuD = "M0 0v1h8v-1h-8zm0 2.97v1h8v-1h-8zm0 3v1h8v-1h-8z";
        private const string pencilD = "M6 0l-1 1 2 2 1-1-2-2zm-2 2l-4 4v2h2l4-4-2-2z";
        private const string bugD = "M3.5 0c-1.19 0-1.98 1.69-1.19 2.5-.09.07-.2.14-.28.22l-1.31-.66a.5.5 0 0 0-.34-.06.5.5 0 0 0-.09.94l1.16.56c-.09.16-.19.33-.25.5h-.69a.5.5 0 0 0-.09 0 .5.5 0 1 0 .09 1h.5c0 .23.02.45.06.66l-.78.41a.5.5 0 1 0 .44.88l.66-.34c.25.46.62.85 1.03 1.09.35-.19.59-.44.59-.72v-1.44a.5.5 0 0 0 0-.09v-.81a.5.5 0 0 0 0-.22c.05-.23.26-.41.5-.41.28 0 .5.22.5.5v.88a.5.5 0 0 0 0 .09v.06a.5.5 0 0 0 0 .09v1.34c0 .27.24.53.59.72.41-.25.79-.63 1.03-1.09l.66.34a.5.5 0 1 0 .44-.88l-.78-.41c.04-.21.06-.43.06-.66h.5a.5.5 0 1 0 0-1h-.69c-.06-.17-.16-.34-.25-.5l1.16-.56a.5.5 0 0 0-.31-.94.5.5 0 0 0-.13.06l-1.31.66c-.09-.08-.19-.15-.28-.22.78-.83 0-2.5-1.19-2.5z";
                
        private const string compassD = "M4 0c-2.2 0-4 1.8-4 4s1.8 4 4 4 4-1.8 4-4-1.8-4-4-4zm0 1c1.66 0 3 1.34 3 3s-1.34 3-3 3-3-1.34-3-3 1.34-3 3-3zm2 1l-3 1-1 3 3-1 1-3zm-2 1.5c.28 0 .5.22.5.5s-.22.5-.5.5-.5-.22-.5-.5.22-.5.5-.5z";
        private const string markerD = "M3 0c-1.66 0-3 1.34-3 3 0 2 3 5 3 5s3-3 3-5c0-1.66-1.34-3-3-3zm0 1c1.11 0 2 .9 2 2 0 1.11-.89 2-2 2-1.1 0-2-.89-2-2 0-1.1.9-2 2-2z";
        private const string zoomInD = "M3.5 0c-1.93 0-3.5 1.57-3.5 3.5s1.57 3.5 3.5 3.5c.61 0 1.19-.16 1.69-.44a1 1 0 0 0 .09.13l1 1.03a1.02 1.02 0 1 0 1.44-1.44l-1.03-1a1 1 0 0 0-.13-.09c.27-.5.44-1.08.44-1.69 0-1.93-1.57-3.5-3.5-3.5zm0 1c1.39 0 2.5 1.11 2.5 2.5 0 .59-.2 1.14-.53 1.56-.01.01-.02.02-.03.03a1 1 0 0 0-.06.03 1 1 0 0 0-.25.28c-.44.37-1.01.59-1.63.59-1.39 0-2.5-1.11-2.5-2.5s1.11-2.5 2.5-2.5zm-.5 1v1h-1v1h1v1h1v-1h1v-1h-1v-1h-1z";
        private const string zoomOutD = "M3.5 0c-1.93 0-3.5 1.57-3.5 3.5s1.57 3.5 3.5 3.5c.61 0 1.19-.16 1.69-.44a1 1 0 0 0 .09.13l1 1.03a1.02 1.02 0 1 0 1.44-1.44l-1.03-1a1 1 0 0 0-.13-.09c.27-.5.44-1.08.44-1.69 0-1.93-1.57-3.5-3.5-3.5zm0 1c1.39 0 2.5 1.11 2.5 2.5 0 .59-.2 1.14-.53 1.56-.01.01-.02.02-.03.03a1 1 0 0 0-.06.03 1 1 0 0 0-.25.28c-.44.37-1.01.59-1.63.59-1.39 0-2.5-1.11-2.5-2.5s1.11-2.5 2.5-2.5zm-1.5 2v1h3v-1h-3z";




        public MarkupString GetImage(SvgName name, int size, string color)
        {
            switch (name)
            {
                case SvgName.Menu:
                    return GetMenu(size, color);
                case SvgName.Edit:
                    return GetPencil(size, color);
                default:
                    return GetBug(size, color);
            }
        }

        private MarkupString GetMenu(int size, string color)
        {
            return GetSvgString(size, menuD, color,"transform=\"translate(0 1)\"");
        }

        private MarkupString GetPencil(int size, string color)
        {
            return GetSvgString(size, pencilD, color);
        }

        private MarkupString GetBug(int size, string color)
        {
            return GetSvgString(size, bugD, color);
        }

        private MarkupString GetSvgString(int size, string dValue, string fillColor, string transform = "")
        {
            return new MarkupString($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 8 8\">\r\n  <path style=\"fill: {fillColor}\" d=\"{dValue}\" {transform} />\r\n</svg>");
        }
    }

    public enum SvgName
    {
        Menu,
        Edit,
        Save,
        Link,
        Bug
    }
}

