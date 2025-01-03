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
                    backgroundColor: "#1976D2", // Set bar color to #1976D2
                    borderColor: "#1976D2", // Border color to match
                    borderWidth: 1, // Add a border width for better styling
                    borderRadius: 5, // Rounded corners for bars
                    hoverBackgroundColor: "#145A86", // Slightly darker color on hover
                    hoverBorderColor: "#145A86", // Border color on hover
                },
            ],
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true,
                    position: "top", // Position the legend at the top
                    align: "end", // Align the legend to the right
                    labels: {
                        font: {
                            size: 12, // Adjust the font size
                            family: "Arial", // Adjust the font family if needed
                        },
                    },
                },
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: "Bulan",
                        font: {
                            size: 14,
                        },
                    },
                    ticks: {
                        color: "#555", // Axis label color
                        font: {
                            size: 12,
                        },
                    },
                },
                y: {
                    title: {
                        display: true,
                        text: "Jumlah",
                        font: {
                            size: 14,
                        },
                    },
                    ticks: {
                        color: "#555", // Axis label color
                        font: {
                            size: 12,
                        },
                        beginAtZero: true, // Ensure Y-axis starts at zero
                        callback: function (value) {
                            return Math.floor(value); // Round down to remove decimals
                        },
                    },
                },
            },
        },
    });

    console.log("Chart successfully rendered.");
}
