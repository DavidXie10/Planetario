function closeModal() {
    document.getElementById("closeModal").click();
}

function changeModal(souvenirId) {
    document.getElementById("yesButton").setAttribute("onclick", "deleteSouvenir(" + souvenirId + ")");
}

function getCookieValue(name) {
    let result = document.cookie.match("(^|[^;]+)\\s*" + name + "\\s*=\\s*([^;]+)")
    return result ? result.pop() : ""
}

function removeItemFromCart(id) {
    let cookieValue = getCookieValue("itemsCart");

    let items = cookieValue.split(',');
    items = items.filter(item => item != id);
    let newCookieValue = items.join(",");

    document.cookie = "itemsCart=" + newCookieValue;
}

function deleteContainer(id) {
    let container = document.getElementById(id);
    container.innerHTML = "";
    container.parentNode.removeChild(container);
}

function deleteSouvenir(id) {
    removeItemFromCart(id);
    deleteContainer(id + "-container");
    if (getCookieValue("itemsCart") == "") {
        document.getElementById("emptyCartMessage").style.display = "block";
        document.getElementById("submit").style.display = "none";
    }
    document.getElementById("closeModal").click();
}

checkCartContent();