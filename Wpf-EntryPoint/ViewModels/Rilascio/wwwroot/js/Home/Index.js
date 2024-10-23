export function initHome() {
    const videos = document.getElementsByClassName('thumbnail-video');

    if (videos.length === 0) {
        return;
    }

    let currentVideoIndex = 0;
    let isPlaying = false;

    const playVideoSequence = async () => {
        while (isPlaying) {
            if (videos.length === 0) return;

            const video = videos[currentVideoIndex];
            video.currentTime = 0;
            await video.play();
            await new Promise(resolve => setTimeout(resolve, 4000));
            video.pause();
            video.currentTime = 0;
            currentVideoIndex = (currentVideoIndex + 1) % videos.length;
        }
    };

    const startSequence = () => {
        isPlaying = true;
        playVideoSequence();
    };

    const stopSequence = () => {
        isPlaying = false;
        Array.from(videos).forEach(video => {
            video.pause();
            video.currentTime = 0;
        });
        currentVideoIndex = 0;
    };
    
    startSequence();
    
}
