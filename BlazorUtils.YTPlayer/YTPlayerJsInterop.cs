using Microsoft.AspNetCore.Components;
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
        private int SlowNetworkRetryCount { get; } = 11;

        private int JsApiDelayMs { get; } = 100;

        public enum YTPlayerState 
        {
            UNSTARTED   = -1, 
            ENDED       = 0,
            PLAYING     = 1,
            PAUSED      = 2,
            BUFFERING   = 3,
            VIDEO_CUED  = 4
        }

        public YTPlayerJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/BlazorUtils.YTPlayer/ytplayer.js").AsTask());
        }

        public async ValueTask LoadYT()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadYTPlayer");

            // retry few times to handle slow networks 
            bool playerReady = false;
            int retries = 0;
            while (!playerReady && retries++ < SlowNetworkRetryCount)
            {
                try
                {
                    await module.InvokeVoidAsync("onYouTubePlayerAPIReady");
                    playerReady = true;
                }
                catch 
                {
                    if (retries > 1) Console.WriteLine($"slow network retry {retries}");
                    await Task.Delay(500);
                }
            }

            await Task.Delay(JsApiDelayMs);
        }

        public async ValueTask LoadVideoById(string videoId)
        {
            var module = await moduleTask.Value;
            bool playerReady = false;
            int retries = 0;
            while (!playerReady && retries++ < SlowNetworkRetryCount)
            {
                try
                {
                    await module.InvokeVoidAsync("loadVideoById", videoId);
                    playerReady = true;
                }
                catch 
                {
                    await Task.Delay(500);
                    if (retries > 1) Console.WriteLine($"slow network retry {retries}");
                }
            }

            await Task.Delay(JsApiDelayMs);
        }

        public async ValueTask PlayVideo()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("playVideo");
            await Task.Delay(JsApiDelayMs);
        }

        public async ValueTask PauseVideo()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("pauseVideo");
            await Task.Delay(JsApiDelayMs);
        }

        public async ValueTask<YTPlayerState> GetPlayerState()
        {
            var module = await moduleTask.Value;
            return (YTPlayerState)await module.InvokeAsync<int>("getPlayerState");
        }

        public async ValueTask TogglePlayPause()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("togglePlayPause");
            await Task.Delay(JsApiDelayMs);
        }

        public async ValueTask<int> GetPlayerHeightPx(ElementReference playerContainer)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("getPlayerHeightPx", playerContainer);
        }
        public async ValueTask<int> GetPlayerWidthPx(ElementReference playerContainer)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<int>("getPlayerWidthPx", playerContainer);
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
