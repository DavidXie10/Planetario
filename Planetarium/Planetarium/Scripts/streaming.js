function checkData(target) {
    let linkInput = document.getElementById("Link");
    let linkValue = linkInput.value;
    if (isValidLink(linkValue)) {
        document.getElementById("buttonSubmit").click();
    } 
}

function isValidLink(link) {
    let isValid = false;
    if (link.includes("/embed/")) {
        isValid = true;
    }
    return isValid;
}

function checkValidLink(target) {
    let errorContainer = document.getElementById("errorMsg");
    let linkInput = document.getElementById("Link");
    linkInput.removeAttribute("style");
    let linkValue = linkInput.value;

    if (isValidLink(linkValue)) {
        errorContainer.style.display = "none";
    } else {
        errorContainer.style.display = "block";
    }
}