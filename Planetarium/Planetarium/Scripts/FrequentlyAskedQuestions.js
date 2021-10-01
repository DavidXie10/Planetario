$(document).ready(function () {
    $("#Category").change(function () {
        $("#Topics").empty();
        $("#selectedTopics").empty();
        if ($("#Category").val() != '') {
            $.ajax({

                type: 'POST',
                url: $("#controllerURL").data("request-url"),
                dataType: 'json',

                data: { category: $("#Category").val() },

                success: function (topics) {

                    $.each(topics, function (i, topic) {
                        $("#Topics").append('<option value=' + topic.Value.replace(" ", "_") + '>' + topic.Text + '</option>');
                    });

                },

                error: function (ex) {
                    alert('Fallo en la recuperación de tópicos' + ex);
                }

            });
            return false;
        } else {
            $("#Topics").empty();
        }
    }

    )
});


$(document).ready(function () {
    $("#Topics").change(function () {
        addTopicButton($("#Topics").val());
    })
});

function addTopicButton(value) {
    let buttonsCard = document.querySelector("#selectedTopics")
    let button = document.createElement("a");
    button.textContent = value;
    button.href = "#";
    button.classList.add("btn", "btn-primary");
    button.style.margin = "1px";
    button.addEventListener("click", () => {
        console.log("hola")
    })
    buttonsCard.appendChild(button);
}




