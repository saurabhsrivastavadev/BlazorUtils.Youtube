﻿using System;
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
            UNSTARTED = -1,
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
        public VideoStreamState StreamState { get; set; } = VideoStreamState.UNSTARTED;
    }
}