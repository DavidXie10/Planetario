function discountApplied(price) {
    let userDiscountInput = document.getElementById("coupon").value;
    let coupon = getCouponByCode(userDiscountInput);
    if (coupon) {
        let discount = price * coupon.Discount;
        var totalPrice = price - discount;
        document.getElementById("discount").innerHTML = "Descuento: - ₡" + discount;
        document.getElementById("totalPrice").innerHTML = "Total a pagar: ₡ " + totalPrice;
    } else {
        document.getElementById("discount").innerHTML = " ";
        document.getElementById("totalPrice").innerHTML = " ";
    }
}

function getFinalPrice(price) {
    let userDiscountInput = document.getElementById("coupon").value;
    let coupon = getCouponByCode(userDiscountInput);
    if (coupon) {
        let discount = price * coupon.Discount;
        return price - discount;
    }

    return 0;
}

function checkData() {
    let cardNumber = document.getElementById("creditCardNumber").value;
    let cardName = document.getElementById("creditCardName").value;
    let month = document.getElementById("monthInput").value;
    let year = document.getElementById("yearInput").value;
    let cvc = document.getElementById("cvcInput").value;
    let typeCard = document.getElementById("typeCard");
    let privacy = document.getElementById("privacy").checked;

    let selectedTypeCard = typeCard.options[typeCard.selectedIndex].text;

    let cardNumberValid = checkValidInput(cardNumber, "#cardNumberError", "Ingrese un número de tarjeta");
    let cardNameValid = checkValidInput(cardName, "#creditCardNameError", "Ingrese el nombre del tarjeta habiente");
    let monthValid = checkValidInput(month, "#monthError", "Ingrese un mes válido");
    let yearValid = checkValidInput(year, "#yearError", "Ingrese un año");
    let cvcValid = checkValidInput(cvc, "#cvcError", "Ingrese el cvc");
    let selectedTypeCardValid = checkValidInput(selectedTypeCard, "#typeCardError", "Seleccione el tipo de tarjeta");
    let privacyValid = checkValidInput(privacy, "#privacyError", "Acepte las políticas de privacidad");

    if (cardNumberValid && cardNameValid && monthValid && yearValid && cvcValid && selectedTypeCardValid && privacyValid) {
        document.getElementById("showModal").click();
    }
}

function checkValidInput(containerValue, errorContainerId, errorMsg) {
    let valid = true;
    if (containerValue == '' || containerValue == '-Seleccionar-') {
        document.querySelector(errorContainerId).innerHTML = errorMsg;
        valid = false;
    } else {
        document.querySelector(errorContainerId).innerHTML = '';
    }
    return valid;
}


function monthLimiter(input) {
    if (input.value < 1) input.value = '';
    if (input.value > 12) input.value = '';
}

function yearLimiter(input) {
    if (input.value < 21) input.value = '';
}


function getCookieValue(name) {
    let result = document.cookie.match("(^|[^;]+)\\s*" + name + "\\s*=\\s*([^;]+)")
    return result ? result.pop() : ""
}

function setCheckoutCookie(idButtonChoice) {
    let cookieValue = getCookieValue("itemsCart");
    document.cookie = "checkoutCookie=" + cookieValue;
    document.cookie = "itemsCart=;";
    document.getElementById(idButtonChoice).click();

    let discount = document.getElementById("discount").innerHTML;
    let discountValue = 0;
    if (discount != "" && discount != " ") {
        discount = discount.split('₡')[1];
        discountValue = parseInt(discount);
    }

    document.cookie = "discount=" + discountValue;

    let coupon = document.getElementById("coupon").value;
    document.cookie = "couponCode=" + coupon;
}