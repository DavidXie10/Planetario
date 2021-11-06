
const BASE_ID_COUNT = '_count';

let checkboxes = document.getElementsByClassName("checkItemDay");
let checkboxesTypes = {};

checkboxesTypes["checkboxesDays"] = document.getElementsByClassName("checkItemDay");
checkboxesTypes["checkboxesComplexityLevel"] = document.getElementsByClassName("checkItemComplexityLevel");
checkboxesTypes["checkboxesTargetAudiences"] = document.getElementsByClassName("checkItemTargetAudience");

function activeCheck(target, index) {
    if (target.value == "0") {
        target.classList.remove("btn-outline-success");
        target.classList.add("btn-success");
        target.value = "1";
    } else {
        target.classList.remove("btn-success");
        target.classList.add("btn-outline-success");
        target.value = "0";
    }
    if (index == 1) {
        toggleDisplay();
        updateStatistics();
    }
}

function toggleDisplay() {
    if (checkIfDaysAreOff()) {
        hide('complexityLevels');
        hide('targetAudiences');
        hide('assistance');
    } else {
        show('complexityLevels');
        show('targetAudiences');
        show('assistance');
    }
}

function checkIfDaysAreOff() {
    let selectedDays = getListSelectedOptions("checkboxesDays");
    return selectedDays.length == 0;
}

function getDayName(inputDate, locale) {
    let date = new Date(inputDate);
    let dateStr = date.toLocaleDateString(locale, { weekday: 'long' });
    return dateStr.charAt(0).toUpperCase() + dateStr.substring(1);
}

function updateStatistics() {
    let result = {};
    let selectedDays = getListSelectedOptions("checkboxesDays");
    let selectedComplexityLevels = getListSelectedOptions("checkboxesComplexityLevel");
    let selectedTargetAudiences = getListSelectedOptions("checkboxesTargetAudiences");
    result = getActivitiesParticipants(selectedDays, selectedComplexityLevels, selectedTargetAudiences);
    hideAll();
    displayCards(result);
}

function displayCards(selectedCards) {
    for (const [key, value] of Object.entries(selectedCards)) {
        show(key);
        document.getElementById(key + BASE_ID_COUNT).innerHTML = value;
    }
}

function show(id){
    document.getElementById(id).style.display = "block";
}

function hide(id){
    document.getElementById(id).style.display = "none";
}

function hideAll() {
    hide('Lunes');
    hide('Martes');
    hide('Miércoles');
    hide('Jueves');
    hide('Viernes');
    hide('Sábado');
    hide('Domingo');
} 

function getActivitiesParticipants(selectedDays, selectedComplexityLevels, selectedTargetAudiences) {
    
    let participants = {};

    const COMPLEXITY_LEVELS = ["", "Básico", "Intermedio", "Avanzado"];
    const TARGET_AUDIENCES = ["", "Infantil", "Juvenil", "Adulto", "Adulto Mayor"];

    //Diccionario <Lunes, Diccionario <Basico, Diccionario <Infantil, 3>>> 
    for (let day = 0; day < selectedDays.length; ++day) {
        participants[selectedDays[day]] = {};
        for (let complexityLevel = 0; complexityLevel < 4; ++complexityLevel) {
            participants[selectedDays[day]][COMPLEXITY_LEVELS[complexityLevel]] = {};
            for (let targetAudience = 0; targetAudience < 5; ++targetAudience) {
                participants[selectedDays[day]][COMPLEXITY_LEVELS[complexityLevel]][TARGET_AUDIENCES[targetAudience]] = 0;
            }
        }
    }

    //1. Lunes
    //2. Lunes -> Basico
    //3. Lunes -> Basico -> Infantil
    //4. Lunes -> Infantil
    if (selectedDays.length != 0) {
        for (let activity in activities) {
            let activityDate = getDayName(activities[activity].StatisticsDate, 'es-ES');
            let activityComplexityLevel = activities[activity].ComplexityLevel;

            activityComplexityLevel = selectedComplexityLevels.length == 0 ? "" : activityComplexityLevel;
            activityComplexityLevel = selectedComplexityLevels.includes(activityComplexityLevel) ? activityComplexityLevel : "";

            let counter = 0;
            let sum = 0;
            let targetAudience = "";

            if (selectedTargetAudiences.length > 0) {
                while (counter < selectedTargetAudiences.length) {
                    console.log("En el while");
                    let selectedTarget = selectedTargetAudiences[counter];
                    console.log(activityDate);
                    console.log(activityComplexityLevel);
                    console.log(selectedTarget);
                    console.log(activities[activity].RegisteredParticipants[selectedTarget]);

                    participants[activityDate][activityComplexityLevel][selectedTarget] += activities[activity].RegisteredParticipants[selectedTarget];
                    console.log(participants);
                    ++counter;
                }
            } else {
                sum = getTotalParticipants(activities[activity].RegisteredParticipants);
                participants[activityDate][activityComplexityLevel][targetAudience] += sum;
            }
            console.log("Saliendooooo de iteracion de for");
            console.log("123");

        }
        console.log("Saliendooooo de for");
    }
    console.log("Saliendooooo");
    console.log(participants);
    
    return participants;
}


function getTotalParticipants(targetAudiencesParticipants) {
    let total = 0;
    for (const [key, value] of Object.entries(targetAudiencesParticipants)) {
        total += value;
    }

    return total;
}

function getListSelectedOptions(checkboxType) {
    let selected = [];
    let checkboxes = checkboxesTypes[checkboxType];
    for (let checkbox = 0; checkbox < checkboxes.length; ++checkbox) {
        if (checkboxes[checkbox].value == '1') {
            selected.push(checkboxes[checkbox].innerHTML);
        }
    }
    return selected;
}

function activeAllButtonsFromRow(checkboxType) {
    toggleButtons(checkboxType, "0");
}

function clearAllButtonsFromRow(checkboxType) {
    toggleButtons(checkboxType, "1");
}

function toggleButtons(checkboxType, value) {
    let checkboxes = checkboxesTypes[checkboxType];
    for (let checkbox = 0; checkbox < checkboxes.length; ++checkbox) {
        checkboxes[checkbox].value = value;
        activeCheck(checkboxes[checkbox], 0);
    }
    toggleDisplay();
    updateStatistics();
}

function executeAllCheckboxesTypes(callback) {
    callback("checkboxesDays");
    callback("checkboxesComplexityLevel");
    callback("checkboxesTargetAudiences");
}