
const BASE_ID_COUNT = '_count';
const DAYS = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
const COMPLEXITY_LEVELS = ["Básico", "Intermedio", "Avanzado"];
const TARGET_AUDIENCES = ["Infantil", "Juvenil", "Adulto", "Adulto Mayor"];

let checkboxesTypes = {};
let result = loadActivitiesParticipants();

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
    let selectedDays = getListSelectedOptions("checkboxesDays");
    let selectedComplexityLevels = getListSelectedOptions("checkboxesComplexityLevel");
    let selectedTargetAudiences = getListSelectedOptions("checkboxesTargetAudiences");
    hideAll();
    displayCards(selectedDays, selectedComplexityLevels, selectedTargetAudiences);
}

function displayCards(selectedDays, selectedComplexityLevels, selectedTargetAudiences) {
    let innerStr = "";
    for (const [day, valueDay] of Object.entries(result)) {
        innerStr = "";
        if (selectedDays.includes(day)) {
            show(day);
            if (selectedComplexityLevels.length != 0 && selectedTargetAudiences.length != 0) {
                innerStr = getTotalByAllFilters(valueDay, selectedComplexityLevels, selectedTargetAudiences);
            } else if (selectedComplexityLevels.length != 0) {
                innerStr = getTotalByComplexityLevel(valueDay, selectedComplexityLevels);
            } else if (selectedTargetAudiences.length != 0) {
                innerStr = getTotalByTargetAudience(valueDay, selectedTargetAudiences);
            } else {
                innerStr = getTotalByDay(valueDay);
            }
            document.getElementById(day + BASE_ID_COUNT).innerHTML = innerStr;
        }
    }
}

function getTotalByAllFilters(valueDay, selectedComplexityLevels, selectedTargetAudiences) {
    let innerStr = "";
    for (const [complexityLevel, valueComplexityLevel] of Object.entries(valueDay)) {
        if (selectedComplexityLevels.includes(complexityLevel)) {
            for (const [targetAudience, valueTargetAudience] of Object.entries(valueComplexityLevel)) {
                if (valueTargetAudience > 0 && selectedTargetAudiences.includes(targetAudience)) {
                    innerStr += addToList(valueTargetAudience, { complexityLevel, targetAudience });
                }
            }
        }
    }
    return innerStr;
}

function getTotalByDay(valueDay) {
    let totalDay = 0;
    let innerStr = "";
    for (const [complexityLevel, valueComplexityLevel] of Object.entries(valueDay)) {
        for (const [targetAudience, valueTargetAudience] of Object.entries(valueComplexityLevel)) {
            totalDay += valueTargetAudience;
        }
    }
    innerStr = addToList(totalDay, {total : "Total"});
    return innerStr;
}

function getTotalByTargetAudience(valueDay, selectedTargetAudiences) {
    let personsPerTargetAudience = {};
    let innerStr = "";
    for (const [complexityLevel, valueComplexityLevel] of Object.entries(valueDay)) {
        for (const [targetAudience, valueTargetAudience] of Object.entries(valueComplexityLevel)) {
            personsPerTargetAudience[targetAudience] = 0
        }
    }
    for (const [complexityLevel, valueComplexityLevel] of Object.entries(valueDay)) {
        for (const [targetAudience, valueTargetAudience] of Object.entries(valueComplexityLevel)) {
            if (selectedTargetAudiences.includes(targetAudience)) {
                if (valueTargetAudience > 0) {
                    personsPerTargetAudience[targetAudience] += valueTargetAudience;
                }
            }
        }
    }
    for (const [targetAudience, valueTargetAudience] of Object.entries(personsPerTargetAudience)) {
        if (valueTargetAudience > 0) {
            innerStr += addToList(valueTargetAudience, { targetAudience });
        }
    }
    return innerStr;
}

function getTotalByComplexityLevel(valueDay, selectedComplexityLevels) {
    let innerStr = "";
    for (const [complexityLevel, valueComplexityLevel] of Object.entries(valueDay)) {
        if (complexityLevel != "" && selectedComplexityLevels.includes(complexityLevel)) {
            let totalAnyTargetAudience = 0;
            for (const [targetAudience, valueTargetAudience] of Object.entries(valueComplexityLevel)) {
                totalAnyTargetAudience += valueTargetAudience;
            }
            if (totalAnyTargetAudience > 0) {
                innerStr += addToList(totalAnyTargetAudience, { complexityLevel });
            }
        }
    }
    return innerStr;
}

function addToList(value, filters) {
    let listItemElement = "<li>";
    if (filters != null) {
        for (const [nameFilter, valueFilter] of Object.entries(filters)) {
            listItemElement += valueFilter + " - ";
        }
        listItemElement = listItemElement.substring(0, listItemElement.length - 3);
        listItemElement += ": ";
    }
    listItemElement += value + "</li>";
    return listItemElement
}

function show(id){
    document.getElementById(id).style.display = "block";
}

function hide(id){
    document.getElementById(id).style.display = "none";
}

function hideAll() {
    for (let day of DAYS) {
        hide(day);
        document.getElementById(day + BASE_ID_COUNT).innerHTML = "";
    }
} 

function loadActivitiesParticipants() {
    
    let participants = {};

    for (let day = 0; day < 7; ++day) {
        participants[DAYS[day]] = {};
        for (let complexityLevel = 0; complexityLevel < 4; ++complexityLevel) {
            participants[DAYS[day]][COMPLEXITY_LEVELS[complexityLevel]] = {};
            for (let targetAudience = 0; targetAudience < 5; ++targetAudience) {
                participants[DAYS[day]][COMPLEXITY_LEVELS[complexityLevel]][TARGET_AUDIENCES[targetAudience]] = 0;
            }
        }
    }
 
    for (let activity of activities) {
        let activityDate = getDayName(activity.StatisticsDate, 'es-ES');
        let activityComplexityLevel = activity.ComplexityLevel;

        let counter = 0;

        while (counter < 4) {
            let selectedTarget = TARGET_AUDIENCES[counter];
            participants[activityDate][activityComplexityLevel][selectedTarget] += activity.RegisteredParticipants[selectedTarget];
            ++counter;
        }
    }

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