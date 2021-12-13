

class TypingLegends {
    constructor(inputIds) {

        //Input Containers
        this.textContainer = document.querySelector(inputIds[0]);
        this.userEntryContainer = document.querySelector(inputIds[1]);
        this.statusContainer = document.querySelector(inputIds[2]);
        
        //Timer Values
        this.seconds = 0;
        this.minutes = 0;
        this.timerActive = false;
        this.timerInterval = "";

        //Global Texts
        this.textOptions = [
            "Neptuno es el octavo planeta en distancia respecto al Sol y el más lejano del sistema solar. Forma parte de los denominados planetas exteriores, y dentro de estos, es uno de los gigantes helados, y es el primero que fue descubierto gracias a predicciones matemáticas.",
            "Saturno es el sexto planeta del sistema solar contando desde el Sol, el segundo en tamaño y masa después de Júpiter y el único con un sistema de anillos visible desde la Tierra. Su nombre proviene del dios romano Saturno. Forma parte de los denominados planetas exteriores o gaseosos.",
            "Un anillo planetario es un anillo de polvo y otras partículas pequeñas que gira alrededor de un planeta.1​ Los más espectaculares y conocidos desde la época telescópica son los anillos de Saturno. Durante mucho tiempo se pensó que Saturno era el único planeta con anillos y su singularidad era un problema.",
            "Júpiter es un cuerpo masivo gaseoso, formado principalmente por hidrógeno y helio, carente de una superficie interior definida. Entre los detalles atmosféricos es notable la Gran Mancha Roja (un enorme anticiclón situado en las latitudes tropicales del hemisferio sur), la estructura de nubes en bandas oscuras y zonas brillantes, y la dinámica atmosférica global determinada por intensos vientos zonales alternantes en latitud y con velocidades de hasta 140 m/s (504 km/h)."
        ]

        //Initialize Game
        this.startGame();

        //Game Stat
        this.mistakes = 0;
    }

    startGame() {
        this.changeText();
        this.seconds = 0;
        this.minutes = 0;
        this.mistakes = 0;
    }

    changeText() {
        let randomOption = Math.floor(Math.random() * this.textOptions.length);
        this.textContainer.textContent = this.textOptions[randomOption];
        this.userEntryContainer.placeholder = this.textOptions[randomOption];
        this.userEntryContainer.value = "";
        if (this.timerInterval != "") {
            this.stopTimer();
            console.log("Se tiene que parar el timer");
        }
    }


    checkChar() {

        if (!this.timerActive) {
            this.startTimer();
            this.timerActive = true;
        }

        let value = this.userEntryContainer.value;
        let textValue = this.textContainer.textContent;
        if (value[value.length - 1] == textValue[value.length - 1]) {
            console.log("Va bien");
            this.changeToGoodText();
        } else {
            console.log("Se las pelo");
            this.changeToBadText();
            this.mistakes += 1;
        }

        let progress = (value.length * 100 / textValue.length);
        return progress;
    }

    changeToGoodText() {
        let statusClasses = this.statusContainer.classList;
        let status = this.statusContainer;
        if (!statusClasses.contains("bg-success")) {
            status.textContent = "Misión va sin problemas";
            statusClasses.remove("bg-danger");
            statusClasses.add("bg-success");
        }
    }

    changeToBadText() {

        let statusClasses = this.statusContainer.classList;
        let status = this.statusContainer;
        if (!statusClasses.contains("bg-danger")) {
            status.textContent = "Estamos perdiendo la misión";
            statusClasses.remove("bg-success");
            statusClasses.add("bg-danger");
        }
    }

    startTimer() {
        this.timerInterval = setInterval(() => {
            console.log(this.timerActive);
            if (this.timerActive) {
                if (this.seconds + 1 == 60) {
                    this.minutes += 1;
                    this.seconds = 0;
                } else {
                    this.seconds++;
                }
            }
            
            console.log(this.minutes + ":" + this.seconds);
        }, 1000);
    }

    stopTimer() {
        
        console.log(this.timerInterval);
        clearInterval(this.timerInterval);
        this.seconds = 0;
        this.minutes = 0;
        this.timerActive = false;
    }

    timer(minutes, seconds) {
        if (seconds + 1 == 60) {
            seconds = 0;
            minutes++;
        } else {
            seconds += 1;
        }

        return [minutes, seconds];
    }

    getStats() {
        let timeStat = this.minutes + ":" + this.seconds;
        let damage = Math.round(this.mistakes * 100 / this.textContainer.textContent.length);
        return [damage, timeStat];
    }
}

