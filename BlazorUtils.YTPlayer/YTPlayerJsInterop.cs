using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorUtils.YTPlayer
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class YTPlayerJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public YTPlayerJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorUtils.YTPlayer/ytplayer.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask LoadYT()
        {
            var module = await moduleTask.Value;
            await module.InvokeAsync<string>("loadYTPlayer");
            await Task.Delay(1000);
            await module.InvokeAsync<string>("onYouTubePlayerAPIReady");
            await Task.Delay(10000);
            await module.InvokeAsync<string>("playVideo");
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
