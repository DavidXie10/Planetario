function checkDataEducationalActivity() {

    let valid = true;

    let userName = document.getElementById("fname").value;
    let userLastName = document.getElementById("lname").value;
    let userMailName = document.getElementById("email").value;
    let cardNumber = document.getElementById("creditCardNumber").value;
    let month = document.getElementById("monthInput").value;
    let year = document.getElementById("yearInput").value;
    let cvc = document.getElementById("cvcInput").value;

    valid = checkValidInputEducationalActivity(userName, "#nameEror", "Por favor ingrese un nombre");
    valid = checkValidInputEducationalActivity(userLastName, "#lastNameError", "Ingrese un DNI");
    valid = checkValidInputEducationalActivity(userMailName, "#emailError", "Ingrese un correo");
    valid = checkValidInputEducationalActivity(cardNumber, "#cardNumberError", "Ingrese un número de tarjeta");
    valid = checkValidInputEducationalActivity(month, "#monthError", "Ingrese un mes válido");
    valid = checkValidInputEducationalActivity(year, "#yearError", "Ingrese un año");
    valid = checkValidInputEducationalActivity(cvc, "#cvcError", "Ingrese el cvc al que se encuentra al reverso de su tarjeta");

    if (valid) {
        document.getElementById("submit").click();
    }
}

function checkValidInputEducationalActivity(containerValue, errorContainerId, errorMsg) {
    let valid = true;
    if (containerValue == '') {
        document.querySelector(errorContainerId).innerHTML = errorMsg;
        valid = false;
        document.getElementById("InvoiceModal").innerHTML = "";
    }
    return valid;
}