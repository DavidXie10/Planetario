$(document).ready(function () {
    $("#Category").change(function () {
        if ($("#Category").val() != '') {
            $.ajax({
                type: 'POST',
                url: $("#controllerURL").data("request-url"),
                dataType: 'json',

                data: { category: $("#Category").val() },

                success: function (topics) {
                    $.each(topics, function (i, topic) {
                        $("#topicSelect").append('<option value=' + topic.Value.replace(" ", "_") + '>' + topic.Text + '</option>');
                    });
                },

                error: function (ex) {
                    alert('Fallo en la recuperación de tópicos' + ex);
                }

            });
            return false;
        } else {
            $("#topicSelect").empty();
        }
    })
});

