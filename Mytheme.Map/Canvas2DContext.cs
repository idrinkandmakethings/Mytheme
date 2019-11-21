
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mytheme.Map
{
    public class Canvas2DContext
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference canvasRef;
        private ElementOffset offset;

        public Canvas2DContext(IJSRuntime jsRuntime, ElementReference canvasRef)
        {
            this.jsRuntime = jsRuntime;
            this.canvasRef = canvasRef;
            GetOffSet();
        }


        public async Task GetOffSet()
        {
            offset = await jsRuntime.InvokeAsync<ElementOffset>("__blazorCanvasInterop.getOffset", canvasRef);
        }

        public async Task<ElementOffset> GetScrollOffset()
        {
            return await jsRuntime.InvokeAsync<ElementOffset>("__blazorCanvasInterop.getScrollOffset", canvasRef);
        }

       public async Task DrawImage(string path)
       {
           await jsRuntime.InvokeAsync<object>("__blazorCanvasInterop.drawImage", canvasRef, path);
       }

        public async Task DrawLine(long startX, long startY, long endX, long endY)
        {
            var scrollOffset = await GetScrollOffset();
            var offLeft = offset.Left - scrollOffset.Left;
            var offTop = offset.Top - scrollOffset.Top;

            await jsRuntime.InvokeAsync<object>("__blazorCanvasInterop.drawLine", canvasRef, startX - offLeft, startY - offTop, endX - offLeft, endY - offTop);
        }

        public async Task SetStrokeStyleAsync(string strokeStyle)
        {
            await jsRuntime.InvokeAsync<object>("__blazorCanvasInterop.setContextPropertyValue", canvasRef, "strokeStyle", strokeStyle);
        }
    }
}
