// Module to work with YouTube Player IFrame API

// Constants 
const YT_PLAYER_STATE_UNLOADED = -1;
const YT_PLAYER_STATE_ENDED = 0;
const YT_PLAYER_STATE_PLAYING = 1;
const YT_PLAYER_STATE_PAUSED = 2;
const YT_PLAYER_STATE_BUFFERING = 3;
const YT_PLAYER_STATE_CUED = 4;

/**
 * Class representing the player state
 * */
class YTPlayerState {

    constructor(success, stateParams) {

        this.success = success;

        if (stateParams.streamState) {
            /** Integer value with below possible YT library namespace variables.
             * YT.PlayerState.ENDED
               YT.PlayerState.PLAYING
               YT.PlayerState.PAUSED
               YT.PlayerState.BUFFERING
               YT.PlayerState.CUED
             * */
            switch (stateParams.streamState) {
                case YT.PlayerState.ENDED:
                    this.streamState = YT_PLAYER_STATE_ENDED;
                    break;
                case YT.PlayerState.PLAYING:
                    this.streamState = YT_PLAYER_STATE_PLAYING;
                    break;
                case YT.PlayerState.PAUSED:
                    this.streamState = YT_PLAYER_STATE_PAUSED;
                    break;
                case YT.PlayerState.BUFFERING:
                    this.streamState = YT_PLAYER_STATE_BUFFERING;
                    break;
                case YT.PlayerState.CUED:
                    this.streamState = YT_PLAYER_STATE_CUED;
                    break;
                default:
                    this.streamState = YT_PLAYER_STATE_UNLOADED;
                    break;
            }
        }

        if (stateParams.loadedVideoUrl) {
            this.loadedVideoUrl = stateParams.loadedVideoUrl;
        }
    }
}

// Module variables
let player;
let isVideoLoaded = false;

export function onYouTubePlayerAPIReady() {

    player = new YT.Player('ytplayer', {
        playerVars: { 'controls': 0, 'rel': 0 }
    });
    console.log('Created YT.Player instance.');
}

export function loadYTPlayer() {

    // Load the IFrame Player API code asynchronously.
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/player_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
}

export function playVideo() {

    if (!isVideoLoaded) {
        console.error('video not yet loaded.');
        return;
    }
    player.playVideo();
}

export function pauseVideo() {

    if (!isVideoLoaded) {
        console.error('video not yet loaded.');
        return;
    }
    player.pauseVideo();
}

export function loadVideoById(videoId) {

    player.cueVideoById({
        'videoId': videoId,
    });
    isVideoLoaded = true;
}

export function getPlayerState() {

    if (!isVideoLoaded) {
        console.error('video not yet loaded.');
        return new YTPlayerState(false);
    }

    return new YTPlayerState(true,
        {
            streamState: player.getPlayerState(),
            loadedVideoUrl: player.getVideoUrl()
        });
}

export function togglePlayPause() {

    if (!isVideoLoaded) {
        console.error('video not yet loaded.');
        return;
    }

    let state = player.getPlayerState();
    if (state === YT.PlayerState.PLAYING) {
        player.pauseVideo();
    } else {
        player.playVideo();
    }
}

export function getPlayerHeightPx(playerHtmlElement) {

    return playerHtmlElement.offsetHeight;
}
export function getPlayerWidthPx(playerHtmlElement) {

    return playerHtmlElement.offsetWidth;
}
