using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorUtils.YTPlayer
{
    /// <summary>
    /// Class for interacting with ytplayer.js script
    /// </summary>
    public class YTPlayerJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private int SlowNetworkRetryCount { get; } = 11;

        private int JsApiDelayMs { get; } = 100;

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
            try
            {
                var state = await module.InvokeAsync<YTPlayerState>("getPlayerState");
                return state;
            }
            catch (Exception) { }
            return null;
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
            try
            {
                return await module.InvokeAsync<int>("getPlayerHeightPx", playerContainer);
            }
            catch (Exception) { }
            return 0;
        }
        public async ValueTask<int> GetPlayerWidthPx(ElementReference playerContainer)
        {
            var module = await moduleTask.Value;
            try
            {
                return await module.InvokeAsync<int>("getPlayerWidthPx", playerContainer);
            }
            catch (Exception) { }
            return 0;
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
