let videoStream;

async function startCamera(videoElementId, useBackCamera) {
    const video = document.getElementById(videoElementId);
    const facingMode = useBackCamera ? "environment" : "user";

    const constraints = {
        video: { facingMode: facingMode },
        audio: false
    };

    try {
        videoStream = await navigator.mediaDevices.getUserMedia(constraints);
        video.srcObject = videoStream;
        video.onplay = () => {
            console.log("Video is playing, ready to capture.");
        };
        video.oncanplay = () => {
            console.log("Video can play, checking dimensions.");
            console.log(`Video dimensions: ${video.videoWidth}x${video.videoHeight}`);
        };
        await video.play();
    } catch (error) {
        console.error("Error starting camera:", error);
        alert("Failed to access the camera.");
    }
}

function capturePhoto(videoElementId, canvasElementId) {
    const video = document.getElementById(videoElementId);
    const canvas = document.getElementById(canvasElementId);
    const context = canvas.getContext('2d');

    // Wait until the video is ready
    if (video.readyState >= 2) { // 2 means HAVE_CURRENT_DATA
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        const dataUrl = canvas.toDataURL('image/jpeg', 0.5);  // Reduce quality

        // Stop the camera stream after capturing
        const stream = video.srcObject;
        const tracks = stream.getTracks();
        tracks.forEach(track => track.stop()); // Stop the camera

        return dataUrl;
    } else {
        console.log("Video not ready yet");
        return null;
    }
}


