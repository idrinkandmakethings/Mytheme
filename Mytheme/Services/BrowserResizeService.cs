using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Mytheme.Services
{
    public class BrowserResizeService
    {
        public static event Func<Task> OnResize;

        [JSInvokable]
        public static async Task OnBrowserResize()
        {
            await OnResize?.Invoke();
        }

        public static async Task RegisterForResize(IJSRuntime runtime)
        {
            await runtime.InvokeAsync<object>("browserResize.register");
        }
        
        public static async Task UnRegisterForResize(IJSRuntime runtime)
        {
            await runtime.InvokeAsync<object>("browserResize.unregister");
        }

        public static async Task<int> GetInnerHeight(IJSRuntime runtime)
        {
            return await runtime.InvokeAsync<int>("browserResize.getInnerHeight");
        }

        public static async Task<int> GetInnerWidth(IJSRuntime runtime)
        {
            return await runtime.InvokeAsync<int>("browserResize.getInnerWidth");
        }
    }
}
