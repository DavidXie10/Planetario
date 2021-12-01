$(document).ready(function () {
    $.ajax({
        dataType: "json",
        url: "https://ubicaciones.paginasweb.cr/provincias.json",
        data: {},
        success: function (data) {
            let html = "<select>  <option value=\"0\" selected>- Sin selección -</option>";
            for (key in data) {
                html += "<option value='" + key + "'>" + data[key] + "</option>";
            }
            html += "</select>";
            $('#province').html(html);
        }
    });
})

$(document).ready(function () {
    $("#province").click(function (evento) {
        $.ajax({
            dataType: "json",
            url: "https://ubicaciones.paginasweb.cr/provincia/" + $('#province').val() + "/cantones.json",
            data: {},
            success: function (data) {
                let html = "<select>  <option value=\"0\" selected>- Sin selección -</option>";
                for (key in data) {
                    html += "<option value='" + key + "'>" + data[key] + "</option>";
                }
                html += "</select>";
                $('#canton').html(html);
            },
            error: function (data) {
                let html = "<select> <option selected>- Sin selección -</option>";
                html += "</select>";
                $('#canton').html(html);
            }
        });
    });
})

$(document).ready(function () {
    $("#canton").click(function (evento) {
        $.ajax({
            dataType: "json",
            url: "https://ubicaciones.paginasweb.cr/provincia/" + $('#province').val() + "/canton/" + $('#canton').val() + "/distritos.json",
            data: {},
            success: function (data) {
                let html = "<select> <option selected>- Sin selección -</option>";
                for (key in data) {
                    html += "<option value='" + key + "'>" + data[key] + "</option>";
                }
                html += "</select>";
                $('#district').html(html);
            },
            error: function (data) {
                let html = "<select> <option selected>- Sin selección -</option></select>";
                $('#district').html(html);
            }
        });
    });
})