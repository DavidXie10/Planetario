////$(document).ready(function () {
////    $("#Category").change(function () {
////        $("#Topics").empty();
////        $("#selectedTopics").empty();
////        $("#inputTopicString").val("");

////        if ($("#Category").val() != '') {
////            $.ajax({

////                type: 'POST',
////                url: $("#controllerURL").data("request-url"),
////                dataType: 'json',

////                data: { category: $("#Category").val() },

////                success: function (topics) {
////                    $("#Topics").append('<option value="" >' + "---Seleccione un topico---" + '</option>');
////                    $.each(topics, function (i, topic) {
////                        $("#Topics").append('<option value=' + topic.Value.replace(" ", "_") + '>' + topic.Text + '</option>');
////                    });

////                },

////                error: function (ex) {
////                    alert('Fallo en la recuperación de tópicos' + ex);
////                }

////            });
////            return false;
////        } else {
////            $("#Topics").empty();
////        }
////    }

////    )
////});