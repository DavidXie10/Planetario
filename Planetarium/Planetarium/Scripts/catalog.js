const CAROUSEL = 'carouselSouvenirImage';
const SOUVENIR_INFORMATION = 'souvenirInformation';
const SOUVENIR_HEADER = 'takeoutModalLabel'

function changeModal(souvenirId) {
    //console.log(souvenirId);
    let souvenirList = document.getElementById(SOUVENIR_INFORMATION);
    souvenirList.innerHTML = "";
    let souvenir = getSouvenirById(souvenirId);
    setCarousel(souvenir);
    setModalInformation(souvenir);
    setHeader(souvenir);
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