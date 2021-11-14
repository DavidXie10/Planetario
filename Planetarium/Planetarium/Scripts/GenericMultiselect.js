class GenericMultiSelect {

    constructor(optionSelectorId, buttonContainerId, inputStringId) {
        this.defaultOptionSelector = document.querySelector(optionSelectorId);
        this.defaultButtonContainer = document.querySelector(buttonContainerId);
        this.defaultInputString = document.querySelector(inputStringId);
    }

    addButton(value) {
        if (!this.isValueInString(value)) {
            //Card Element
            let button = document.createElement("a");

            //Button properties
            button.textContent = String(value).replaceAll("_", " ");
            button.classList.add("btn", "btn-success");
            button.style.margin = "2px";

            //Removing element onClick
            button.addEventListener("click", (triggeredEvent) => {

                //Button to delete
                let buttonToDelete = triggeredEvent.target;

                //Value (string) to delete
                let value = buttonToDelete.textContent;
                //Deleting the value from the string container
                this.deleteElementString(value);

                //Deleting from document
                buttonToDelete.parentNode.removeChild(buttonToDelete);

            });

            //Adding custom button to card
            this.defaultButtonContainer.appendChild(button);

            //Adding button value to string
            this.addElementToArray(String(value));
        }

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
        let newValues = String(currentValue).replaceAll(value.replaceAll(" ", "_") + "|", "");

        //Updating the string
        this.defaultInputString.value = newValues;

    }

    isValueInString(value) {
        let isInString = true;

        //Parsing value
        let parsedValue = String(value).replaceAll(" ", "_");
        parsedValue += "|";

        if (!this.defaultInputString.value.includes(parsedValue)) {
            isInString = false;
        }

        return isInString;
    }
}

function addTopicButton(element) {
    if (element != "") {
        multiSelectTopics.addButton(element);
    }
}

function addCategoryButton(element) {
    element = String(element).replaceAll(" ", "_");
    if (element != "") {
        multiSelectCategory.addButton(element);
    }
}

function addAudienceButton(element) {
    if (element != "") {
        multiSelectTargetAudience.addButton(element);
    }
}

function addLanguageButton(element) {
    element = String(element).replaceAll(" ", "_");
    if (element != "") {
        multiSelectLanguages.addButton(element);
    }
}

function isVirtual(value) {
    if (value == "Virtual" || value == "Bimodal") {
        document.querySelector("#inputLink").style.display = "block";
    } else {
        document.querySelector("#inputLink").style.display = "none";
    }
}