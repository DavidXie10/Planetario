

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
        }
    })

 
    

    let calendarphenomenon = $("#calendarioFenomenos").fullCalendar({
        initialView: 'dayGridMonth',
        header:
        {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay,list'
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
        }

    })
    showInitialCalendar();
})



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
    button.classList.remove("bt");
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