
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mytheme.Map
{
    class LeafletInterop
    {
        private static readonly string _BaseObjectContainer = "window.leafletBlazor";

        public static ValueTask CreateImageMap(IJSRuntime jsRuntime, string mapId, string mapUrl, long height, long width)
        {
            return jsRuntime.InvokeVoidAsync($"{_BaseObjectContainer}.createImageMap", mapId, mapUrl, width, height);
        }

        public static async Task AddMarker(IJSRuntime jsRuntime, double lat, double lng, MarkupString content)
        {
            await jsRuntime.InvokeVoidAsync($"{_BaseObjectContainer}.addMarker", lat, lng, content.Value);
        }
    }
}
