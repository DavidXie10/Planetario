const CAROUSEL = 'carouselSouvenirImage';
const SOUVENIR_INFORMATION = 'souvenirInformation';
const SOUVENIR_HEADER = 'takeoutModalLabel';
const BUTTON_CONTAINER = 'buttonContainer';
const COUNTER_CONTAINER = 'counter';

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
    let counter = document.getElementById(COUNTER_CONTAINER);
    counter.innerHTML = '<span class="minus bg-dark" onclick="decrement(' + souvenir.SouvenirId + ')">-</span>';
    counter.innerHTML += '<input type = "number" class="count" name = "' + souvenir.SouvenirId + '" value = "0" id = "' + souvenir.SouvenirId + '" />';
    counter.innerHTML += '<span class="plus bg-dark" onclick="increment(' + souvenir.SouvenirId + ',' + souvenir.Stock + ')">+</span>';
}

function setButton(souvenirId) {
    let button = document.getElementById(BUTTON_CONTAINER);
    button.innerHTML = '<button type="button" id="' + souvenirId + '" class="btn btn-success w-50" style=" margin: 10px; margin-top:5px" onclick= updateCookie(' + souvenirId + ')>Agregar al carrito</button>';
}

function setCarousel(souvenir) {
    let souvenirList = document.getElementById(CAROUSEL);
    souvenirList.innerHTML = '<div class="carousel-inner">' +
        '<div class="carousel-item active"> <img src="/images/Souvenirs/' + souvenir.ImagesRef[0] + '" class="d-block w-100 carousel-Img" alt="..."></div >';

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
    souvenirInformation.innerHTML = "<p><strong>Descripción:</strong> " + souvenir.Description + " </p> <p><strong>Categoria:</strong> " + souvenir.Category + "</p><p><strong>Precio:</strong> " + souvenir.Price + "</p>";
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

function updateCookie(value) {
    let cookieValue = getCookieValue("itemsCart");
    document.cookie = "itemsCart=" + cookieValue + value + ",";
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