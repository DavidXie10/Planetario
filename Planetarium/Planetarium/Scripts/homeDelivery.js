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

function checkData() {
    let provinceId = document.getElementById("province");
    let cantonId = document.getElementById("canton");
    let districtId = document.getElementById("district");
    let exactDirection = document.getElementById("exactDirection").value;
    let postalCode = document.getElementById("postalCode").value;

    let selectedProvince = provinceId.options[provinceId.selectedIndex].text;
    let selectedCanton = cantonId.options[cantonId.selectedIndex].text;
    let selectedDistrict = districtId.options[districtId.selectedIndex].text;

    let provinceValid = checkValidInput(selectedProvince, "#provinceError", "Seleccione una provincia");
    let cantonValid = checkValidInput(selectedCanton, "#cantonError", "Seleccione un cantón");
    let districtValid = checkValidInput(selectedDistrict, "#districtError", "Seleccione un distrito");
    let exactDirectionValid = checkValidInput(exactDirection, "#exactDirectionError", "Ingrese su dirección exacta");
    let postalCodeValid = checkValidInput(postalCode, "#postalCodeError", "Ingrese su código postal");

    if (provinceValid && cantonValid && districtValid && exactDirectionValid && postalCodeValid) {
        document.getElementById("submit").click();
    }
}

function checkValidInput(containerValue, errorContainerId, errorMsg) {
    let valid = true;

    if (containerValue == '' || containerValue == '- Sin selección -') {
        document.querySelector(errorContainerId).innerHTML = errorMsg;
        valid = false;
    } else {
        document.querySelector(errorContainerId).innerHTML = '';
    }
    return valid;
}