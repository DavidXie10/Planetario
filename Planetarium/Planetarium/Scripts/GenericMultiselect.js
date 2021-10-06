
let defaultInputString = document.querySelector("#defaultInputString");
let defaultOptionSelector = document.querySelector("#defaultOptionSelector");
let defaultButtonContainer = document.querySelector("#defaultButtonContainer");


function addButton(value) {

    //Card Element
    let button = document.createElement("a");

    //Button properties
    button.textContent = String(value).replace("_", " ");
    button.classList.add("btn", "btn-success");
    button.style.margin = "2px";

    //Removing element onClick
    button.addEventListener("click", deleteOnEvent);

    //Adding custom button to card
    defaultButtonContainer.appendChild(button)

    //Adding button value to string
    addElementToArray(String(value))
}

function deleteOnEvent(triggeredEvent) {

    //Button to delete
    let buttonToDelete = triggeredEvent.target;

    //Value (string) to delete
    let value = buttonToDelete.textContent;

    //Deleting the value from the string container
    deleteElementString(String(value));

    //Deleting from document
    buttonToDelete.parentNode.removeChild(buttonToDelete)
}

function addElementToArray(value) {

    //Test
    console.log("Valor para ser insertado: " + value)

    //Hidden input string current value
    let values = defaultInputString.value;

    //Test
    console.log("Valores previos: " + values);

    //Checking existance
    if (values.includes(value) == 0) {
        values += value + "|";
    }

    //Updating the value
    defaultInputString.value = values;

    //Test
    console.log("Valor despues del cambio: " + defaultInputString.value);
}

function deleteElementString(value) {

    //Hidden input string current value
    let currentValue = defaultInputString.value;

    //Deleting the value
    let newValues = String(currentValue).replace(value + "|", "");

    //Updating the string
    defaultInputString.value = newValues;
    
}
