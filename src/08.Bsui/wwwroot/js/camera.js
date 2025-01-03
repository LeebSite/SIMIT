let videoStream;

async function startCamera(videoElementId) {
    const video = document.getElementById(videoElementId);

    const constraints = {
        video: { facingMode: "user" },
        audio: false,
    };

    try {
        videoStream = await navigator.mediaDevices.getUserMedia(constraints);
        video.srcObject = videoStream;
        await video.play();
    } catch (error) {
        console.error("Error starting camera:", error);
        alert("Failed to access the camera.");
    }
}

function capturePhoto(videoElementId) {
    const video = document.getElementById(videoElementId);

    if (!video) {
        console.error("Video element not found!");
        return null;
    }

    const canvas = document.createElement("canvas");
    document.body.appendChild(canvas); // Tambahkan ke DOM sementara

    try {
        const context = canvas.getContext("2d");

        if (video.readyState >= 2) { // Video is ready
            canvas.width = video.videoWidth;
            canvas.height = video.videoHeight;
            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            const dataUrl = canvas.toDataURL("image/jpeg", 0.5);

            // Stop video stream
            const stream = video.srcObject;
            const tracks = stream.getTracks();
            tracks.forEach(track => track.stop());

            return dataUrl;
        } else {
            console.log("Video not ready yet.");
            return null;
        }
    } catch (error) {
        console.error("Error capturing photo:", error);
        return null;
    } finally {
        document.body.removeChild(canvas); // Hapus elemen canvas dari DOM
    }
}


function resetCamera(videoElementId) {
    const video = document.getElementById(videoElementId);
    video.srcObject = null;

    if (videoStream) {
        videoStream.getTracks().forEach((track) => track.stop());
    }
}
