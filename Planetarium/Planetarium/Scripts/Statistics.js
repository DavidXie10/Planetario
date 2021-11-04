
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
        updateStatistics();
    }
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

    // document.getElementById('result').innerHTML = result + (result != 1 ? " personas" : " persona");
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
    for (let day = 0; day < selectedDays.length; ++day) {
        participants[selectedDays[day]] = 0;
    }

    if (selectedDays.length != 0 || selectedComplexityLevels.length != 0 || selectedTargetAudiences != 0) {
        for (let activity in activities) {
            let existsDay = false;
            let existsComplexityLevel = false;
            let existsTargetAudience = false;

            let activityDate = getDayName(activities[activity].StatisticsDate, 'es-ES');
            existsDay = selectedDays.length == 0 || selectedDays.includes(activityDate);
            if (existsDay) {
                existsComplexityLevel = selectedComplexityLevels.length == 0 || selectedComplexityLevels.includes(activities[activity].ComplexityLevel);

                if (existsComplexityLevel) {
                    let targetAudiences = activities[activity].TargetAudience;
                    let targetAudiencesCount = targetAudiences.length;
                    let targetElement = 0;
                    if (selectedTargetAudiences.length != 0) {
                        while (!existsTargetAudience && targetElement < targetAudiencesCount) {
                            existsTargetAudience = selectedTargetAudiences.includes(targetAudiences[targetElement]);
                            ++targetElement;
                        }
                    } else {
                        existsTargetAudience = true;
                    }
                }
            }

            if (selectedDays.includes(activityDate)) {
                participants[activityDate] += (existsDay && existsComplexityLevel && existsTargetAudience ? activities[activity].RegisteredParticipants : 0);
            }
        }
    }

    return participants;
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
    updateStatistics();
}

function executeAllCheckboxesTypes(callback) {
    callback("checkboxesDays");
    callback("checkboxesComplexityLevel");
    callback("checkboxesTargetAudiences");
}