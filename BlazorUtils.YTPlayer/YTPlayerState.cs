using System;
using System.Threading.Tasks;

namespace BlazorUtils.YTPlayer
{
    /// <summary>
    /// Class holding the player state
    /// </summary>
    public class YTPlayerState 
    {
        public enum VideoStreamState
        {
            UNLOADED = -1,
            ENDED = 0,
            PLAYING = 1,
            PAUSED = 2,
            BUFFERING = 3,
            CUED = 4
        }

        /// <summary>
        /// Is player state fetched successfuly 
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Video player stream state
        /// </summary>
        public VideoStreamState StreamState { get; set; } = VideoStreamState.UNLOADED;

        /// <summary>
        /// URL of the video which is loaded, either paused or playing.
        /// </summary>
        public string LoadedVideoUrl { get; set; }

        /// <summary>
        /// Video ID extracted from the above Video URL
        /// </summary>
        public string LoadedVideoId { get; set; }
    }
}
