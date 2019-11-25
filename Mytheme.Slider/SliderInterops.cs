using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Mytheme.Slider
{
    public class SliderInterops
    {
        public static async Task Initailize(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeAsync<object>("sliderInterops.initialize");
        }
    }
}
