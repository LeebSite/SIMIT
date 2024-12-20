function initializeCalendar(selector, dates) {
    const calendarElement = document.querySelector(selector);
    if (!calendarElement) {
        console.error("Calendar element not found");
        return;
    }

    new FullCalendar.Calendar(calendarElement, {
        initialView: 'dayGridMonth',
        events: dates && dates.length > 0 ? dates.map(date => ({
            start: date,
            display: 'background',
            backgroundColor: 'green',
        })) : [] // If no dates, pass an empty events array
    }).render();
}
