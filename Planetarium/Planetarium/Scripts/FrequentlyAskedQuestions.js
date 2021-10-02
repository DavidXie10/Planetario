$(document).ready(function () {
    $("#Category").change(function () {
        $("#Topics").empty();
        $("#selectedTopics").empty();
        $("#inputTopicString").val("");

        if ($("#Category").val() != '') {
            $.ajax({

                type: 'POST',
                url: $("#controllerURL").data("request-url"),
                dataType: 'json',

                data: { category: $("#Category").val() },

                success: function (topics) {
                    $("#Topics").append('<option value="" >' + "---Seleccione un topico---" + '</option>');
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
        if ($("#Topics").val() != '')
            addTopicButton($("#Topics").val());
            addElementToArray($("#Topics").val());
    })
});

function addTopicButton(value) {
    let buttonsCard = document.querySelector("#selectedTopics")
    let button = document.createElement("a");
    button.textContent = String(value).replace("_", " ");
    button.href = "#";
    button.classList.add("btn", "btn-success");
    button.style.margin = "2px";
    button.addEventListener("click", (event) => {
        let thisButton = event.target;
        thisButton.parentNode.removeChild(thisButton);
    })
    buttonsCard.appendChild(button)
}

function addElementToArray(element) {
    console.log(element);
    let inputTopicString = document.querySelector("#inputTopicString");
    console.log(inputTopicString);
    let values = inputTopicString.value;
    console.log("Prev: " + values)
    if (values.includes(element) == 0) {
        console.log("No se encuentra")
        values += element + "|";
    } else {
        console.log("Se encuentra")
    }
    inputTopicString.value = values;
    console.log("Inner: " + inputTopicString.value);
}


