// This is a JavaScript module that is loaded on demand. 
// It can export any number of functions, and may import 
// other JavaScript modules if required.

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

    player.loadVideoById({
        'videoId': videoId,
    });
    isVideoLoaded = true;
}

export function togglePlayPause() {

    if (!isVideoLoaded) {
        console.error('video not yet loaded.');
        return;
    }

    let state = player.getPlayerState();
    if (state === 1) {
        player.pauseVideo();
    } else {
        player.playVideo();
    }
}
