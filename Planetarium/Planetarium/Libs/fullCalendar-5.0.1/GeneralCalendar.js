class CalendarInitializer {
    constructor(dataOriginGenral, dataOriginPhenomenom) {
        this.dataOriginGeneral = dataOriginGenral;
        this.dataOriginPhenomenom = dataOriginPhenomenom;
        document.addEventListener('DOMContentLoaded', this.loadCalendarEvents('generalCalendar', this.dataOriginGeneral));
        document.addEventListener('DOMContentLoaded', this.loadCalendarEvents('phenomenomCalendar', this.dataOriginPhenomenom));
        setTimeout(showInitialCalendar, 1000);
    }

    loadCalendarEvents(idCalendar, dataOrigin) {
        return async () => {
            const eventsLoaded = await this.getEventsFromDB(dataOrigin);
            var calendarEl = document.getElementById(idCalendar);
            var calendar = new FullCalendar.Calendar(calendarEl, {
                headerToolbar: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'dayGridMonth,dayGridWeek,listMonth'
                },
                locale: 'es',
                titleFormat: { year: 'numeric', month: 'long'  },
                events: eventsLoaded,
                eventClick: function (event) {
                    eventClicked(event)
                }
            });
            calendar.render();
        }
    }

    async getEventsFromDB(dataOrigin) {
        const response = await fetch(dataOrigin);
        const events = await response.json();
        const parsedEvents = [];
        events.forEach(event => {
            parsedEvents.push({
                title: event.Title,
                start: event.Date,
                color: event.Color,
                extendedProps: {
                    description: event.Description,
                    link: event.Link,
                    date: event.Date
                }
            })
        })
        return parsedEvents;
    }

    reloadCalendars() {
        var generalCalendar = document.getElementById("generalCalendar").render();
        var phenomenomCalendar = document.getElementById("phenomenomCalendar").render();
    }
}

function eventClicked(event) {
    event = event.event._def;

    let titleContainerId = "#calendarEventLongTitle";
    let descriptionContainerId = "#calendarEventDescription";
    let linkButtonId = "#linkButton";
    let buttonId = "#modalToggle";

    let title = document.querySelector(titleContainerId);
    let description = document.querySelector(descriptionContainerId);
    let linkButton = document.querySelector(linkButtonId);
    let button = document.querySelector(buttonId);

    title.innerHTML = "<strong>" + event.title + "</strong> <br /> <h5>" + event.extendedProps.date + "</h5>";
    description.innerHTML = verifyField(event.extendedProps.description, "Sin descripción");
    linkButton.setAttribute("href", verifyField(event.extendedProps.link, "/EducationalActivity/ListActivities"));

    button.click();
}

function verifyField(fieldValue, defaultValue) {
    let verifiedField = fieldValue;
    if (verifiedField == "" || verifiedField == null) {
        verifiedField = defaultValue;
    }
    return verifiedField;
}

function changeCalendar(target) {
    //Calendar toggles
    let generalCal = document.querySelector("#generalCalToggle");
    let phenomenonCal = document.querySelector("#phenomenomCalToggle");

    //Calendar containers
    let generalCalContainer = document.querySelector("#generalContainer");
    let phenomenonCalContainer = document.querySelector("#phenomenonContainer");

    if (target == generalCal) {
        activeButton(generalCal);
        deactiveButton(phenomenonCal);
        showContainer(generalCalContainer);
        hideContainer(phenomenonCalContainer);
    } else {
        activeButton(phenomenonCal);
        deactiveButton(generalCal);
        showContainer(phenomenonCalContainer);
        hideContainer(generalCalContainer);
    }
}

function showInitialCalendar() {
    let generalCalendarToggle = document.querySelector("#generalCalToggle");
    generalCalendarToggle.click();
}

function activeButton(button) {
    button.classList.remove("btn-primary");
    button.classList.remove("btn-secondary");
    button.classList.add("btn-primary");
}

function deactiveButton(button) {
    button.classList.remove("btn-primary");
    button.classList.remove("btn-secondary");
    button.classList.add("btn-secondary");
}

function showContainer(container) {
    container.style.display = "block";
}

function hideContainer(container) {
    container.style.display = "none";
}