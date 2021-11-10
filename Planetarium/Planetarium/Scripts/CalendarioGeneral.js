
$(document).ready(function () {
    let calendarGeneral = $("#calendarioGeneral").fullCalendar({
        //TODO: Fijarse si funciona en Chrome, en Edge si sale en español
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
        themeSystem: 'bootstrap',
        initialView: 'dayGridMonth',
        
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,list'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día',
            list: 'Lista'
        },
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: $("#controllerURLGeneral").data("request-url"),
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    let loadedEvents = [];
                    $.each(result, function (i, event){
                        loadedEvents.push(
                            {
                                title: event.Title,
                                description: event.Description,
                                start: event.Date,
                                color: event.Color
                            });
                    })
                    callback(loadedEvents);
                }
                
            })
        },
        eventClick: function clicked(event) {
            eventClicked(event);
        }
    })

    function eventClicked(event) {

        console.log(event);

        //Initial id
        let modalId = "#calendarEventModal";
        let titleContainerId = "#calendarEventLongTitle";
        let descriptionContainerId = "#calendarEventDescription";
        let buttonId = "#modalToggle";

        //Html elements
        let modal = document.querySelector(modalId);
        let title = document.querySelector(titleContainerId);
        let description = document.querySelector(descriptionContainerId);
        let button = document.querySelector(buttonId);

        //Adding info to modal
        title.childNodes[0].textContent = event.title;
        description.textContent = event.description;

        //show modal
        button.click();
    }

 
    

    let calendarphenomenon = $("#calendarioFenomenos").fullCalendar({

        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
        initialView: 'dayGridMonth',

        header:
        {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,list'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día',
            list: 'Lista'
        },
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: $("#controllerURLPhenomenon").data("request-url"),
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    let loadedEvents = [];
                    $.each(result, function (i, event) {
                        loadedEvents.push(
                            {
                                title: event.Title,
                                description: event.Description,
                                start: event.Date,
                                color: event.Color
                            });
                    })
                    callback(loadedEvents);
                }

            })
        },
        eventClick: function clicked(event) {
            eventClicked(event);
        }

    })
    showInitialCalendar();
})


function eventClicked(target) {
    console.log(target);
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
    //Calendar containers
    let generalCalContainer = document.querySelector("#generalContainer");
    let phenomenonCalContainer = document.querySelector("#phenomenonContainer");

    showContainer(generalCalContainer);
    hideContainer(phenomenonCalContainer);
}

function activeButton(button) {
    button.classList.remove("btn-prima");
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