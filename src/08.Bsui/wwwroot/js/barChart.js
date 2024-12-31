let existingChart = null; // Global variable to track the existing chart

function renderBarChart(labels, values) {
    console.log("renderBarChart called with labels:", labels, "and values:", values);

    const canvas = document.getElementById("myChart");
    if (!canvas) {
        console.error("Canvas element with id 'myChart' not found! DOM might not be fully loaded.");
        return;
    }

    const context = canvas.getContext("2d");
    if (!context) {
        console.error("Failed to acquire 2D context for the canvas!");
        return;
    }

    // Destroy the existing chart if it exists
    if (existingChart) {
        console.log("Destroying existing chart...");
        existingChart.destroy();
    }

    const defaultLabels = ["No Data"];
    const defaultValues = [0];

    const finalLabels = labels.length > 0 ? labels : defaultLabels;
    const finalValues = values.length > 0 ? values : defaultValues;

    // Create a new chart instance and store it in the global variable
    existingChart = new Chart(context, {
        type: "bar",
        data: {
            labels: finalLabels,
            datasets: [
                {
                    label: "Jumlah Logbooks",
                    data: finalValues,
                    backgroundColor: [
                        "#4CAF50", "#2196F3", "#FF9800", "#E91E63", "#9C27B0",
                        "#3F51B5", "#00BCD4", "#8BC34A", "#FFC107", "#FF5722",
                        "#795548", "#607D8B"
                    ],
                },
            ],
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: true },
                title: {
                    display: true,
                    text: "Jumlah Logbooks per Bulan",
                },
            },
            scales: {
                x: { title: { display: true, text: "Bulan" } },
                y: { title: { display: true, text: "Jumlah" }, beginAtZero: true },
            },
        },
    });

    console.log("Chart successfully rendered.");
}
