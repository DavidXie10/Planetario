const CAROUSEL = 'carouselSouvenirImage';
const SOUVENIR_INFORMATION = 'souvenirInformation';
const SOUVENIR_HEADER = 'takeoutModalLabel';
const BUTTON_CONTAINER = 'buttonContainer';
const COUNTER_CONTAINER = 'counter';
const countOccurrences = (array, val) => array.reduce((number, compareValue) => (compareValue == val ? number + 1 : number), 0);

function changeModal(souvenirId) {
    let souvenirList = document.getElementById(SOUVENIR_INFORMATION);
    souvenirList.innerHTML = "";
    let souvenir = getSouvenirById(souvenirId);
    setCounter(souvenir);
    setCarousel(souvenir);
    setModalInformation(souvenir);
    setHeader(souvenir);
    setButton(souvenirId);
}

function setCounter(souvenir) {
    let stock = souvenir.Stock;
    let cookieValue = getCookieValue("itemsCart");
    let cookieArray = cookieValue.split(',');
    let countElements = countOccurrences(cookieArray, souvenir.SouvenirId);

    stock -= countElements;
    let counter = document.getElementById(COUNTER_CONTAINER);
    if (stock > 0) {
        document.getElementById(BUTTON_CONTAINER).style.display = "block";
        counter.innerHTML = '<span class="minus bg-dark" onclick="decrement(' + souvenir.SouvenirId + ')">-</span>';
        counter.innerHTML += '<input type = "number" class="count" name = "' + souvenir.SouvenirId + '" value = 1 id = "' + souvenir.SouvenirId + '" />';
        counter.innerHTML += '<span class="plus bg-dark" onclick="increment(' + souvenir.SouvenirId + ',' + stock + ')">+</span>';
    } else {
        counter.innerHTML = '<span class="text-danger"> ¡Producto agotado! </span>'
        document.getElementById(BUTTON_CONTAINER).style.display = "none";
    }
}

function setButton(souvenirId) {
    let button = document.getElementById(BUTTON_CONTAINER);
    button.innerHTML = '<button type="button" id="' + "button-" + souvenirId + '" class="btn btn-success w-50" style=" margin: 10px; margin-top:5px" onclick= updateCookie(' + souvenirId + ')>Agregar al carrito</button>';
}

function setCarousel(souvenir) {
    let souvenirList = document.getElementById(CAROUSEL);
    souvenirList.innerHTML = '<div class="carousel-inner w-100">' +
        '<div class="carousel-item active" style="width:20rem;height:20rem;"> <img src="/images/Souvenirs/' + souvenir.ImagesRef[0] + '" class="d-block w-100 carousel-Img" alt="..." style="margin-left:4rem"></div >';

    for (let index = 1; index < souvenir.ImagesRef.length; ++index) {
        souvenirList.innerHTML += '<div class="carousel-item"> <img src="/images/Souvenirs/' + souvenir.ImagesRef[index] + '" class="d-block w-100 carousel-Img" alt = "..." > </div>';
    }
    souvenirList.innerHTML += '</div>';

    if (souvenir.ImagesRef.length > 1) {
        souvenirList.innerHTML += '<a class="carousel-control-prev" href="#carouselSouvenirImage" role="button" data-slide="prev"> <span class="carousel-control-prev-icon" aria-hidden="true"></span> <span class="sr-only">Previous</span> </a>';
        souvenirList.innerHTML += '<a class="carousel-control-next" href="#carouselSouvenirImage" role="button" data-slide="next"> <span class="carousel-control-next-icon" aria-hidden="true"></span> <span class="sr-only">Next</span> </a>';
    }

}

function setModalInformation(souvenir) {
    let souvenirInformation = document.getElementById(SOUVENIR_INFORMATION);
    souvenirInformation.innerHTML = "<p><strong>Descripción:</strong> " + souvenir.Description + " </p> <p><strong>Categoria:</strong> " + souvenir.Category + "</p><p><strong>Precio:</strong> ₡" + souvenir.Price + "</p>";
}

function setHeader(souvenir) {
    let souvenirInformation = document.getElementById(SOUVENIR_HEADER);
    souvenirInformation.innerHTML = souvenir.Name;
}

function getSouvenirById(souvenirId) {
    for (let souvenir of allSouvenirs) {
        if (souvenir.SouvenirId == souvenirId) {
            return souvenir;
        }
    }
    return null;
}

function getCookieValue(name) {
    let result = document.cookie.match("(^|[^;]+)\\s*" + name + "\\s*=\\s*([^;]+)")
    return result ? result.pop() : ""
}

function countUnique(cookieArray) {
    let itemsCount = 0;
    let uniqueItems = [];
    for (let item of cookieArray) {
        if (item != "" && !uniqueItems.includes(item)) {
            itemsCount += 1;
            uniqueItems.push(item);
        }
    }

    return itemsCount;
}

function getItemsCount() {
    let cookieValue = getCookieValue("itemsCart");
    let cookieArray = cookieValue.split(',');
    let counter = countUnique(cookieArray);
    return counter;
}

function updateCookie(value) {
    let count = parseInt(document.getElementById(value).value);
    if (count != 0) {
        let cookieValue = getCookieValue("itemsCart");

        let addToCookieValue = "";
        for (let index = 0; index < count; ++index) {
            addToCookieValue += value + ",";
        }

        document.cookie = "itemsCart=" + cookieValue + addToCookieValue;
        document.getElementById("cartCounter").innerHTML = getItemsCount();
    }

    document.getElementById("closeModal").click();
}

function increment(inputId, maximum) {
    let input = document.getElementById(inputId);
    if (parseInt(input.value) + 1 <= maximum) {
        input.value = parseInt(input.value) + 1;
    }
}

function decrement(inputId) {
    let input = document.getElementById(inputId);
    if (parseInt(input.value) - 1 >= 0) {
        input.value = input.value - 1;
    }
}

document.getElementById("cartCounter").innerHTML = getItemsCount();