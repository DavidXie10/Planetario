function checkDataEducationalActivity() {


    let userName = document.getElementById("fname").value;
    let userLastName = document.getElementById("lname").value;
    let userMailName = document.getElementById("email").value;
    let cardNumber = document.getElementById("creditCardNumber").value;
    let cardName = document.getElementById("creditCardName").value;
    let month = document.getElementById("monthInput").value;
    let year = document.getElementById("yearInput").value;
    let cvc = document.getElementById("cvcInput").value;
    let typeCard = document.getElementById("typeCard");
    let privacy = document.getElementById("privacy").checked;

    let selectedTypeCard = typeCard.options[typeCard.selectedIndex].text;

    let nameValid = checkValidInputEducationalActivity(userName, "#nameEror", "Por favor ingrese un nombre");
    let lastNameValid = checkValidInputEducationalActivity(userLastName, "#lastNameError", "Ingrese un DNI");
    let emailValid = checkValidInputEducationalActivity(userMailName, "#emailError", "Ingrese un correo");
    let cardNumberValid = checkValidInput(cardNumber, "#cardNumberError", "Ingrese un número de tarjeta");
    let cardNameValid = checkValidInput(cardName, "#creditCardNameError", "Ingrese el nombre del tarjeta habiente");
    let monthValid = checkValidInput(month, "#monthError", "Ingrese un mes válido");
    let yearValid = checkValidInput(year, "#yearError", "Ingrese un año");
    let cvcValid = checkValidInput(cvc, "#cvcError", "Ingrese el cvc");
    let selectedTypeCardValid = checkValidInput(selectedTypeCard, "#typeCardError", "Seleccione el tipo de tarjeta");
    let privacyValid = checkValidInput(privacy, "#privacyError", "Acepte las políticas de privacidad");


    if (nameValid && lastNameValid && emailValid && cardNumberValid && cardNameValid && monthValid && yearValid && cvcValid && selectedTypeCardValid && privacyValid) {
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