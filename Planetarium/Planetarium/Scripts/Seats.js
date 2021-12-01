﻿class SeatGenerator {
    constructor(seatTableContainerId, maxParticipants, url) {
        this.seatTableContainerId = seatTableContainerId;
        this.maxParticipants = parseInt(maxParticipants);
        this.url = url;
        this.generateSeatArray();
    }

    async generateSeatArray() {
        this.reservedSeats = await this.fetchReservedSeats();
        let seats = document.getElementById(this.seatTableContainerId)
        let maxColumns = Math.ceil(Math.sqrt(this.maxParticipants));
        let createdSeats = 0;

        let space = "\n"

        for (let i = 1; createdSeats < this.maxParticipants; i++) {
            let tr = document.createElement("tr")
            for (let j = 1; j <= maxColumns && (createdSeats < this.maxParticipants); j++) {
                let td = document.createElement("td")

                let value = i + "-" + j;
                space += value + "\t\t";
                let seat = "";
                if (this.reservedSeats.indexOf(value) >= 0) {
                    seat = this.createSeat(value, "btn-danger");
                } else {
                    seat = this.createSeat(value, "btn-light");
                }
                
                td.appendChild(seat);
                tr.appendChild(td);
                createdSeats++;
            }
            space += "\n"
            seats.appendChild(tr)
        }
    }

    async fetchReservedSeats() {
        const response = await fetch(this.url);
        const seatsFromDB = await response.json();
        return seatsFromDB;
    }
    

    createSeat(value, type) {
        //Creating the element
        let seat = document.createElement("button");

        //Giving the value
        let textContent = document.createTextNode(value);
        seat.appendChild(textContent);

        //Setting the attributes
        seat.classList.add("btn", type);

        //Setting the event listener
        if (type != "btn-danger") {
            seat.addEventListener("click", event => {
                this.buttonClicked(event.target);
            })
        }
        

        return seat;
    }

    buttonClicked(target) {
        //this.clearSelection();
        this.selectButton(target);
        this.enableButton();
    }


    selectButton(button) {
        button.classList.remove("btn-success");
        button.classList.remove("btn-light");
        button.classList.add("btn-success");
        this.addValueToContainer(button);
    }

    addValueToContainer(button) {
        let container = document.getElementById("selectedSeat");
        container.value = button.textContent;
    }

    clearContainerValue() {
        let container = document.getElementById("selectedSeat");
        container.value = "";
    }

    unselectButton(button) {
        if (!button.classList.contains("btn-danger")) {
            button.classList.remove("btn-success");
            button.classList.remove("btn-light");
            button.classList.add("btn-light");
        }
        
    }

    clearSelection() {
        let table = document.getElementById(this.seatTableContainerId);
        let rows = table.rows.length;
        let cols = table.rows[0].cells.length;
        let reviewedButtons = 0;
        for (let i = 0; i < rows; i++) {
            for (let j = 0; j < cols && reviewedButtons < this.maxParticipants; j++) {
                let button = table.rows[i].cells[j].childNodes[0];
                this.unselectButton(button)
                reviewedButtons++;
            }
        }
    }

    getSeats() {
        let selectedSeats = [];
        let table = document.getElementById(this.seatTableContainerId);
        let rows = table.rows.length;
        let cols = table.rows[0].cells.length;
        let reviewedButtons = 0;
        for (let i = 0; i < rows; i++) {
            for (let j = 0; j < cols && reviewedButtons < this.maxParticipants; j++) {
                let button = table.rows[i].cells[j].childNodes[0];
                if (this.isSelected(button)) {
                    console.log(button)
                    selectedSeats.push(button.textContent);
                }
                reviewedButtons++;
            }
        }
        return selectedSeats;
    }

    isSelected(button) {
        let isSelected = false;
        if (button.classList.contains("btn-success")) {
            isSelected = true;
        }
        return isSelected;
    }

    enableButton() {
        let continueButton = document.getElementById("continueButton");
        continueButton.classList.remove("btn-success");
        continueButton.classList.remove("btn-light");
        continueButton.classList.add("btn-success");
    }

    diableButton() {
        let continueButton = document.getElementById("continueButton");
        continueButton.classList.remove("btn-success");
        continueButton.classList.remove("btn-light");
        continueButton.classList.add("btn-light");
    }
}


function enableButton() {
    let container = document.getElementById("selectedSeat");
    container.value = button.textContent;
}

function clearSelection() {
    seatsGen.clearSelection();
    seatsGen.diableButton();
    seatsGen.clearContainerValue();
}

function isSelected(button) {
    let isSelected = false;
    if (button.classList.contains("btn-success")) {
        isSelected = true;
    }
    return isSelected;
}

function checkData() {
    let stringInput = document.getElementById("selectedSeat");
    let seats = seatsGen.getSeats();
    let parsedSeats = "";
    seats.forEach(element => {
        parsedSeats += element + ",";
    });

    stringInput.value = parsedSeats;

    if (stringInput.value != '') {
        //document.getElementById("Reservar").click();
        

    }
    console.log(stringInput.value);
}

