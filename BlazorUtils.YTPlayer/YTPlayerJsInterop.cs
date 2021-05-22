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

        public async ValueTask LoadYT()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadYTPlayer");
            await Task.Delay(1000);
            await module.InvokeVoidAsync("onYouTubePlayerAPIReady");
            await Task.Delay(1000);
        }

        public async ValueTask LoadVideoById(string videoId)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadVideoById", videoId);
        }

        public async ValueTask PlayVideo()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("playVideo");
        }

        public async ValueTask PauseVideo()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("pauseVideo");
        }

        public async ValueTask TogglePlayPause()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("togglePlayPause");
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
