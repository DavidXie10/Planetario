$(document).ready(function () {

    let calendar = $("#calendarioFenomenos").fullCalendar({
        initialView: 'dayGridMonth',
        events: function (start, end, timezone, callback) {
            $.ajax({
                url: $("#controllerURL").data("request-url"),
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    let loadedEvents = [];
                    $.each(result, function (i, event) {
                        loadedEvents.push(
                            {
                                title: event.Title,
                                description: event.Description,
                                start: event.Date
                                color: event.Color
                            });
                    })
                    callback(loadedEvents);
                }

            })
        }
    })

})