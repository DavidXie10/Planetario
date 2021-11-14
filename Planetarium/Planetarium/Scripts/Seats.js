

class SeatGenerator {
    constructor(seatTableContainerId, maxParticipants) {
        this.seatTableContainerId = seatTableContainerId;
        this.maxParticipants = parseInt(maxParticipants);
    }

    generateSeatArray() {
        let seats = document.getElementById(this.seatTableContainerId)
        let maxColumns = Math.ceil(Math.sqrt(this.maxParticipants));
        console.log("Max Cols: " + maxColumns)
        let createdSeats = 0;

        let space = "\n"

        for (let i = 1; createdSeats < this.maxParticipants; i++) {
            let tr = document.createElement("tr")
            for (let j = 1; j <= maxColumns && (createdSeats < this.maxParticipants); j++) {
                let td = document.createElement("td")

                let value = i + "-" + j;
                space += value + "\t\t";
                let seat = this.createSeat(value);
                td.appendChild(seat);
                tr.appendChild(td);
                createdSeats++;
            }
            space += "\n"
            seats.appendChild(tr)
        }

        console.log(seats)
        console.log(space)
        console.log(createdSeats);
    }

    createSeat(value) {
        //Creating the element
        let seat = document.createElement("button");

        //Giving the value
        let textContent = document.createTextNode(value);
        seat.appendChild(textContent);

        //Setting the attributes
        seat.classList.add("btn", "btn-light");

        //Setting the event listener
        seat.addEventListener("click", event => {

        })

        return seat;
    }

    clearSelection() {
        return pain;
    }


}

