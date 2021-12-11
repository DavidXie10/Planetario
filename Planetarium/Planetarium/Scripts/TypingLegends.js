

class TypingLegends {
    constructor(inputIds) {
        console.log("se crea el juego")
        this.textContainer = document.querySelector(inputIds[0]);
        this.userEntryContainer = document.querySelector(inputIds[1]);
        this.statusContainer = document.querySelector(inputIds[2]);
        this.startGame();
    }

    startGame() {
        this.textContainer.textContent = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur";
        this.userEntryContainer.placeholder = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur";
    }

    checkChar() {
        let value = this.userEntryContainer.value;
        let textValue = this.textContainer.textContent;
        if (value[value.length - 1] == textValue[value.length - 1]) {
            console.log("Va bien");
            this.changeToGoodText();
        } else {
            console.log("Se las pelo");
            this.changeToBadText();
        }
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
}

