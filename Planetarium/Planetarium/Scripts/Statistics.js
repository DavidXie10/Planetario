
const BASE_ID_COUNT = '_count';
const DAYS = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
const COMPLEXITY_LEVELS = ["", "Básico", "Intermedio", "Avanzado"];
const TARGET_AUDIENCES = ["", "Infantil", "Juvenil", "Adulto", "Adulto Mayor"];

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
    result = loadActivitiesParticipants(selectedComplexityLevels, selectedTargetAudiences);
    hideAll();
    displayCards(result, selectedDays, selectedComplexityLevels, selectedTargetAudiences);
}

function displayCards(result, selectedDays, selectedComplexityLevels, selectedTargetAudiences) {
    let innerStr = "";
    for (const [key, value] of Object.entries(result)) {
        innerStr = "";
        if (selectedDays.includes(key)) {
            show(key);
            if (selectedComplexityLevels.length != 0 && selectedTargetAudiences != 0) {
                for (const [key2, value2] of Object.entries(value)) {
                    if (selectedComplexityLevels.includes(key2)) {
                        for (const [key3, value3] of Object.entries(value2)) {
                            if (value3 > 0) {
                                let listItemElement = "<li>";
                                listItemElement += key2 + " - " + key3 + ": " + value3;
                                listItemElement += "</li>";
                                innerStr += listItemElement;
                            }
                        }
                    }
                }
            } else if (selectedComplexityLevels.length != 0) {
                let sum = {}
                for (const [key2, value2] of Object.entries(value)) {
                    if (key2 != "") {
                        let totalAnyTargetAudience = 0;
                        for (const [key3, value3] of Object.entries(value2)) {
                            totalAnyTargetAudience += value3;
                        }
                        sum[key2] = totalAnyTargetAudience;
                    }
                }
                for (const [key2, value2] of Object.entries(sum)) {
                    if (value2 > 0) {
                        let listItemElement = "<li>";
                        listItemElement += key2 + ": " + value2;
                        listItemElement += "</li>";
                        innerStr += listItemElement;
                    }
                }
            } else if (selectedTargetAudiences != 0) {
                let sum = {};
                for (const [key2, value2] of Object.entries(value)) {
                    for (const [key3, value3] of Object.entries(value2)) {
                        sum[key3] = 0
                    }
                }
                for (const [key2, value2] of Object.entries(value)) {
                    for (const [key3, value3] of Object.entries(value2)) {
                        if (value3 > 0) {
                            sum[key3] += value3;
                            console.log(key3);
                            console.log(value3);
                        }
                    }
                }
                for (const [key2, value2] of Object.entries(sum)) {
                    if (value2 > 0) {
                        let listItemElement = "<li>";
                        listItemElement += key2 + ": " + value2;
                        listItemElement += "</li>";
                        innerStr += listItemElement;
                    }
                }
            } else {
                let totalDay = 0;
                for (const [key2, value2] of Object.entries(value)) {
                    for (const [key3, value3] of Object.entries(value2)){
                        totalDay += value3;
                    }
                }
                let listItemElement = "<li>";
                listItemElement += totalDay;
                listItemElement += "</li>";
                innerStr += listItemElement;
            }
            document.getElementById(key + BASE_ID_COUNT).innerHTML = innerStr;
        }
    }
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

function loadActivitiesParticipants(selectedComplexityLevels, selectedTargetAudiences) {
    
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

        activityComplexityLevel = selectedComplexityLevels.length == 0 ? "" : activityComplexityLevel;
        activityComplexityLevel = selectedComplexityLevels.includes(activityComplexityLevel) ? activityComplexityLevel : "";

        let counter = 0;
        let sum = 0;
        let targetAudience = "";

        if (selectedTargetAudiences.length > 0) {
            while (counter < selectedTargetAudiences.length) {
                let selectedTarget = selectedTargetAudiences[counter];
                participants[activityDate][activityComplexityLevel][selectedTarget] += activity.RegisteredParticipants[selectedTarget];
                ++counter;
            }
        } else {
            sum = getTotalParticipants(activity.RegisteredParticipants);
            participants[activityDate][activityComplexityLevel][targetAudience] += sum;
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