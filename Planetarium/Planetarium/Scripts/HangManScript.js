let time = 0;       //Tiempo
let lives = 0;       // Lives
let intentos;

function startGame(TotalLives) {
    lives = TotalLives;
    intentos = TotalLives;
    time = setTimeout(time);
    comments();
}


window.onload = function () {
    window.scrollTo(0, 0);
    document.getElementById("instructionsModal").click();

    var alphabet = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
        'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
        'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

    var categories;         // Array of topics
    var chosenCategory;     // Selected catagory
    var getHint;          // Word getHint
    var word;              // Selected word
    var guess;             // Geuss
    var geusses = [];      // Stored geusses
    var counter;           // Count correct geusses
    var space;              // Number of spaces in word '-'

    // Get elements
    var showLives = document.getElementById("mylives");
    var showCatagory = document.getElementById("scatagory");
    var getHint = document.getElementById("hint");
    var showClue = document.getElementById("clue");

    // create alphabet ul
    var buttons = function () {
        myButtons = document.getElementById('buttons');
        letters = document.createElement('ul');

        for (var i = 0; i < alphabet.length; i++) {
            letters.id = 'alphabet';
            list = document.createElement('li');
            list.id = 'letter';
            list.innerHTML = alphabet[i];
            check();
            myButtons.appendChild(letters);
            letters.appendChild(list);
        }
    }

    // Select Catagory
    var selectCat = function () {
        if (chosenCategory === categories[0]) {
            catagoryName.innerHTML = "La categoría es: Planetas ";
        } else if (chosenCategory === categories[1]) {
            catagoryName.innerHTML = "La categoría es: Nombres de científicos famosos ";
        } else if (chosenCategory === categories[2]) {
            catagoryName.innerHTML = "La categoría es: Conceptos de astronomía ";
        }
    }

    // Create geusses ul
    result = function () {
        wordHolder = document.getElementById('hold');
        correct = document.createElement('ul');

        for (var i = 0; i < word.length; i++) {
            correct.setAttribute('id', 'my-word');
            guess = document.createElement('li');
            guess.setAttribute('class', 'guess');
            if (word[i] === "-") {
                guess.innerHTML = "-";
                space = 1;
            } else {
                guess.innerHTML = "_";
            }

            geusses.push(guess);
            wordHolder.appendChild(correct);
            correct.appendChild(guess);
        }
    }

    // Show lives
    comments = function () {
        showLives.innerHTML = "Tiene " + lives + " intentos";
        if (lives < 1) {
            showLives.innerHTML = "Perdiste";
            document.getElementById("modalImage").innerHTML = `<img src="../images/GamesImages/—Pngtree—spaceship outer space astronaut_6222644.png" width="80" height="60" alt="" style="border-radius: 0% ;width: 11% ;height: 29% ;text-align: center;">`;
            document.getElementById("resutModal").click();
        }
        for (var i = 0; i < geusses.length; i++) {
            if (counter + space === geusses.length) {
                showLives.innerHTML = "Ganaste";
                document.getElementById("modalImage").innerHTML = `<img src="../images/GamesImages/astronaut drives t-rex.png" width="80" height="60" alt="" style="border-radius: 0% ;width: 16% ;height: 29% ;text-align: center;">`;
                document.getElementById("resutModal").click();
            }
        }
    }

    // Animate man
    var animate = function () {
        var drawMe = lives;
        drawArray[drawMe]();
    }

    // OnClick Function
    check = function () {
        list.onclick = function () {
            var geuss = (this.innerHTML);
            this.setAttribute("class", "active");
            this.onclick = null;
            for (var i = 0; i < word.length; i++) {
                if (word[i] === geuss) {
                    geusses[i].innerHTML = geuss;
                    counter += 1;
                }
            }
            var j = (word.indexOf(geuss));
            if (j === -1) {
                lives -= 1;
                comments();
            } else {
                comments();
            }
        }
    }

    // Play
    play = function () {
        window.scrollTo(0, 0);
        categories = [
            ["MERCURIO", "VENUS", "TIERRA", "MARTE", "JUPITER", "SATURNO", "URANO", ],
            ["NEWTON", "ARISTOTELES", "COPERNICO", "TESLA", "ARQUIMEDES"],
            ["DOPPLER", "ECLIPSE", "GRAVEDAD", "PERIGEO", "HELIOCENTRISMO"]
        ];

        chosenCategory = categories[Math.floor(Math.random() * categories.length)];
        word = chosenCategory[Math.floor(Math.random() * chosenCategory.length)];
        word = word.replace(/\s/g, "-");
        buttons();

        geusses = [];
        lives = intentos;
        counter = 0;
        space = 0;
        result();
        comments();
        selectCat();
    }

    play();

    // Hint

    hint.onclick = function () {

        hints = [
            ["Es el planeta del sistema solar más cercano al Sol y el más pequeño. Forma parte de los denominados planetas interiores y carece de satélites naturales al igual que Venus.", "El segundo planeta del sistema solar en orden de proximidad al Sol y el tercero en cuanto a tamaño en orden ascendente después de Mercurio y Marte. Al igual que Mercurio, carece de satélites naturales.", "Es la tercera órbita más interna. Es el más denso y el quinto mayor de los ocho planetas del sistema solar. También es el mayor de los cuatro terrestres o rocosos.", "El cuarto planeta en orden de distancia al Sol y el segundo más pequeño del sistema solar, después de Mercurio. Recibió su nombre en homenaje al dios de la guerra de la mitología romana (Ares en la mitología griega), y también es conocido como «el planeta rojo»", "Es el planeta más grande del sistema solar y el quinto en orden de lejanía al Sol.3​ Es un gigante gaseoso que forma parte de los denominados planetas exteriores.", "Es el sexto planeta del sistema solar contando desde el Sol, el segundo en tamaño y masa después de Júpiter y el único con un sistema de anillos visible desde la Tierra.", "Es el séptimo planeta del sistema solar, el tercero de mayor tamaño, y el cuarto más masivo."],
            ["Es autor de los 'Philosophiæ naturalis principia mathematica', más conocidos como los Principia, donde describe la ley de la gravitación universal y estableció las bases de la mecánica clásica mediante las leyes que llevan su nombre.", "Fue un filósofo, polímata y científico nacido en la ciudad de Estagira, al norte de Antigua Grecia. Es considerado junto a Platón, el padre de la filosofía occidental.", "Fue matemático, astrónomo, jurista, físico, clérigo católico, gobernador, diplomático y economista. Junto con sus extensas responsabilidades, la astronomía figuraba como poco más que una distracción.", "No el de los carros", "Entre sus avances en física se encuentran sus fundamentos en hidrostática, estática y la explicación del principio de la palanca."],
            ["Es el cambio aparente de la frecuencia de una onda cuando existe un movimiento relativo entre la fuente emisora y el observador. El físico y matemático austríaco Christian Doppler fue quien postuló esta teoría en 1841.", "Se trata del bloqueo total o parcial de un cuerpo celeste por otro.", "Se trata de una fuerza física mutua de la naturaleza que hace que dos cuerpos se atraigan entre sí. Depende de la masa de los cuerpos y de la distancia que los separa.", "Hablamos del punto de la órbita de la Luna más cercano a la Tierra. En el caso de los planetas, hablaríamos de perihelio.","Es una concepción cosmológica antigua abanderada por Aristarco de Samos en el siglo III antes de Cristo y posteriormente postulada por Nicolas Copérnico en el S.XVI, que consideraba que el Sol era el centro del universo -y no la Tierra- (geocentrismo)."]
        ];

        var catagoryIndex = categories.indexOf(chosenCategory);
        var hintIndex = chosenCategory.indexOf(word);
        showClue.innerHTML = "Pista: " + hints[catagoryIndex][hintIndex];
    };

    // Reset

    document.getElementById('reset').onclick = function () {
        document.getElementById("modalImage").innerHTML = "";
        correct.parentNode.removeChild(correct);
        letters.parentNode.removeChild(letters);
        showClue.innerHTML = "";
        play();
    }
}


