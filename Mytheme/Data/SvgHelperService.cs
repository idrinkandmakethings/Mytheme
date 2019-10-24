using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Mytheme.Data
{
    public class SvgHelperService
    {
        public MarkupString Menu { get; set; }

        public MarkupString GetMenu(int size, string css)
        {
            return new MarkupString(
                $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 8 8\">\r\n  <path style=\"{css}\" d=\"M0 0v1h8v-1h-8zm0 2.97v1h8v-1h-8zm0 3v1h8v-1h-8z\" transform=\"translate(0 1)\" />\r\n</svg>");
        }
    }
}

