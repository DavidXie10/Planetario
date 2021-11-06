$(document).ready(function () {
    $("#Category").change(function () {
        //$("#topicSelect").empty();
        //Solo se quiere agregar
        //$("#topicsContainer").empty();
        //$("#inputTopicString").val("");

        if ($("#Category").val() != '' && $("#inputCategoryString").val().includes($("#Category").val().replace(" ", "_"))) {
            $.ajax({
                type: 'POST',
                url: $("#controllerURL").data("request-url"),
                dataType: 'json',

                data: { category: $("#Category").val() },

                success: function (topics) {
                    //$("#topicSelect").append('<option value="" >' + "---Seleccione un topico---" + '</option>');
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

