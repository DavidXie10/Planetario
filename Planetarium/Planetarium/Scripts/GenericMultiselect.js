class GenericMultiSelect {

    constructor(optionSelectorId, buttonContainerId, inputStringId) {
        this.defaultOptionSelector = document.querySelector(optionSelectorId);
        this.defaultButtonContainer = document.querySelector(buttonContainerId);
        this.defaultInputString = document.querySelector(inputStringId);
        console.log('holis')
    }

    addButton(value) {

        //Card Element
        let button = document.createElement("a");

        //Button properties
        button.textContent = String(value).replace("_", " ");
        button.classList.add("btn", "btn-success");
        button.style.margin = "2px";

        //Removing element onClick
        button.addEventListener("click", (triggeredEvent) => {

            //Button to delete
            let buttonToDelete = triggeredEvent.target;

            //Value (string) to delete
            let value = buttonToDelete.textContent;

            //Deleting the value from the string container
            this.deleteElementString(String(value));

            //Deleting from document
            buttonToDelete.parentNode.removeChild(buttonToDelete);

        });

        //Adding custom button to card
        this.defaultButtonContainer.appendChild(button);

        //Adding button value to string
        addElementToArray(String(value));
    }

    deleteOnEvent(triggeredEvent) {

        //Button to delete
        let buttonToDelete = triggeredEvent.target;

        //Value (string) to delete
        let value = buttonToDelete.textContent;

        //Deleting the value from the string container
        this.deleteElementString(String(value));

        //Deleting from document
        buttonToDelete.parentNode.removeChild(buttonToDelete);
    }

    addElementToArray(value) {

        //Hidden input string current value
        let values = this.defaultInputString.value;

        //Checking existance
        if (values.includes(value) == 0) {
            values += value + "|";
        }

        //Updating the value
        this.defaultInputString.value = values;
    }

    deleteElementString(value) {

        //Hidden input string current value
        let currentValue = this.defaultInputString.value;

        //Deleting the value
        let newValues = String(currentValue).replace(value + "|", "");

        //Updating the string
        this.defaultInputString.value = newValues;

    }
}