using Microsoft.AspNetCore.Components;

namespace Mytheme.Services
{
    public class SvgHelperService
    {
        private const string menuD = "viewBox=\"0 0 8 8\"><path d =\"M0 0v1h8v-1h-8zm0 2.97v1h8v-1h-8zm0 3v1h8v-1h-8z\" transform=\"translate(0 1)\"";
        private const string editD = "viewBox=\"0 0 576 512\"><path d=\"M402.3 344.9l32-32c5-5 13.7-1.5 13.7 5.7V464c0 26.5-21.5 48-48 48H48c-26.5 0-48-21.5-48-48V112c0-26.5 21.5-48 48-48h273.5c7.1 0 10.7 8.6 5.7 13.7l-32 32c-1.5 1.5-3.5 2.3-5.7 2.3H48v352h352V350.5c0-2.1.8-4.1 2.3-5.6zm156.6-201.8L296.3 405.7l-90.4 10c-26.2 2.9-48.5-19.2-45.6-45.6l10-90.4L432.9 17.1c22.9-22.9 59.9-22.9 82.7 0l43.2 43.2c22.9 22.9 22.9 60 .1 82.8zM460.1 174L402 115.9 216.2 301.8l-7.3 65.3 65.3-7.3L460.1 174zm64.8-79.7l-43.2-43.2c-4.1-4.1-10.8-4.1-14.8 0L436 82l58.1 58.1 30.9-30.9c4-4.2 4-10.8-.1-14.9z\"";
        private const string bugD = "viewBox=\"0 0 8 8\"><path d\"M3.5 0c-1.19 0-1.98 1.69-1.19 2.5-.09.07-.2.14-.28.22l-1.31-.66a.5.5 0 0 0-.34-.06.5.5 0 0 0-.09.94l1.16.56c-.09.16-.19.33-.25.5h-.69a.5.5 0 0 0-.09 0 .5.5 0 1 0 .09 1h.5c0 .23.02.45.06.66l-.78.41a.5.5 0 1 0 .44.88l.66-.34c.25.46.62.85 1.03 1.09.35-.19.59-.44.59-.72v-1.44a.5.5 0 0 0 0-.09v-.81a.5.5 0 0 0 0-.22c.05-.23.26-.41.5-.41.28 0 .5.22.5.5v.88a.5.5 0 0 0 0 .09v.06a.5.5 0 0 0 0 .09v1.34c0 .27.24.53.59.72.41-.25.79-.63 1.03-1.09l.66.34a.5.5 0 1 0 .44-.88l-.78-.41c.04-.21.06-.43.06-.66h.5a.5.5 0 1 0 0-1h-.69c-.06-.17-.16-.34-.25-.5l1.16-.56a.5.5 0 0 0-.31-.94.5.5 0 0 0-.13.06l-1.31.66c-.09-.08-.19-.15-.28-.22.78-.83 0-2.5-1.19-2.5z\"";
        private const string mapD = "viewBox=\"0 0 576 512\"><path d=\"M560.02 32c-1.96 0-3.98.37-5.96 1.16L384.01 96H384L212 35.28A64.252 64.252 0 0 0 191.76 32c-6.69 0-13.37 1.05-19.81 3.14L20.12 87.95A32.006 32.006 0 0 0 0 117.66v346.32C0 473.17 7.53 480 15.99 480c1.96 0 3.97-.37 5.96-1.16L192 416l172 60.71a63.98 63.98 0 0 0 40.05.15l151.83-52.81A31.996 31.996 0 0 0 576 394.34V48.02c0-9.19-7.53-16.02-15.98-16.02zM224 90.42l128 45.19v285.97l-128-45.19V90.42zM48 418.05V129.07l128-44.53v286.2l-.64.23L48 418.05zm480-35.13l-128 44.53V141.26l.64-.24L528 93.95v288.97z\"";
        private const string pageD = "viewBox=\"0 0 384 512\"><path d=\"M369.9 97.9L286 14C277 5 264.8-.1 252.1-.1H48C21.5 0 0 21.5 0 48v416c0 26.5 21.5 48 48 48h288c26.5 0 48-21.5 48-48V131.9c0-12.7-5.1-25-14.1-34zM332.1 128H256V51.9l76.1 76.1zM48 464V48h160v104c0 13.3 10.7 24 24 24h104v288H48z\"";
        private const string saveD = "viewBox=\"0 0 448 512\"><path d=\"M433.941 129.941l-83.882-83.882A48 48 0 0 0 316.118 32H48C21.49 32 0 53.49 0 80v352c0 26.51 21.49 48 48 48h352c26.51 0 48-21.49 48-48V163.882a48 48 0 0 0-14.059-33.941zM272 80v80H144V80h128zm122 352H54a6 6 0 0 1-6-6V86a6 6 0 0 1 6-6h42v104c0 13.255 10.745 24 24 24h176c13.255 0 24-10.745 24-24V83.882l78.243 78.243a6 6 0 0 1 1.757 4.243V426a6 6 0 0 1-6 6zM224 232c-48.523 0-88 39.477-88 88s39.477 88 88 88 88-39.477 88-88-39.477-88-88-88zm0 128c-22.056 0-40-17.944-40-40s17.944-40 40-40 40 17.944 40 40-17.944 40-40 40z\"";
        private const string folderD = "viewBox=\"0 0 576 512\"><path d=\"M527.9 224H480v-48c0-26.5-21.5-48-48-48H272l-64-64H48C21.5 64 0 85.5 0 112v288c0 26.5 21.5 48 48 48h400c16.5 0 31.9-8.5 40.7-22.6l79.9-128c20-31.9-3-73.4-40.7-73.4zM48 118c0-3.3 2.7-6 6-6h134.1l64 64H426c3.3 0 6 2.7 6 6v42H152c-16.8 0-32.4 8.8-41.1 23.2L48 351.4zm400 282H72l77.2-128H528z\"";
        private const string arrowD = "viewBox=\"0 0 8 8\"><path d=\"M4 0c-2.21 0-4 1.79-4 4s1.79 4 4 4 4-1.79 4-4-1.79-4-4-4zm0 1l3 3-3 3v-2h-3v-2h3v-2z\"";

        public MarkupString GetImage(SvgName name, int size, string color)
        {
            switch (name)
            {
                case SvgName.Menu:
                    return GetSvgString(size, menuD, color);
                case SvgName.CircleArrowRight:
                    return GetSvgString(size, arrowD, color);
                case SvgName.Edit:
                    return GetSvgString(size, editD, color);
                case SvgName.Map:
                    return GetSvgString(size, mapD, color);
                case SvgName.Page:
                    return GetSvgString(size, pageD, color);
                case SvgName.Save:
                    return GetSvgString(size, saveD, color);
                case SvgName.Folder:
                    return GetSvgString(size, folderD, color);
                default:
                    return GetSvgString(size, bugD, color);
            }
        }


        private MarkupString GetSvgString(int size, string dValue, string fillColor)
        {
            return new MarkupString($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\"  {dValue} style=\"fill: {fillColor}\" /></svg>");
        }
    }

    public enum SvgName
    {
        Menu,
        CircleArrowRight,
        Edit,
        Save,
        Map,
        Page,
        Folder,
        Link,
        Bug
    }
}

