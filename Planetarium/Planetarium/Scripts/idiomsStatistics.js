const ALL_IDIOMS_LIST_CONTAINER = 'allIdiomsListContainer';
const LEFT_COLUMN_LIST = 'leftColumn';
const RIGHT_COLUMN_LIST = 'rightColumn';
const EMPLOYEES_MODAL_BODY = 'employees-list';
const INPUT_IDIOMS = 'inputLanguageString';
const NOT_FOUND_MESSAGE = 'notFoundMessage';
const IDIOMS_INTERSECTION_CONTAINER = 'idiomsIntersectionContainer';
const SPAN_COUNT = 'count';

function updateIdiomsList() {
    let idioms = getSelectedIdioms();
    let employeesBySelectedIdioms = getEmployeesByIdioms(idioms);
    let employeesCount = Object.keys(employeesBySelectedIdioms).length;

    if (employeesCount > 0) {
        hide(NOT_FOUND_MESSAGE);
        show(IDIOMS_INTERSECTION_CONTAINER);

        document.getElementById(SPAN_COUNT).innerHTML = ' (' + employeesCount + ')';
        setModal(employeesBySelectedIdioms);
    } else if (document.getElementById(INPUT_IDIOMS).value != "") {
        hide(IDIOMS_INTERSECTION_CONTAINER);
        show(NOT_FOUND_MESSAGE);
    } else {
        hide(IDIOMS_INTERSECTION_CONTAINER);
        hide(NOT_FOUND_MESSAGE);
    }
}

function getEmployeesByIdioms(idioms) {
    let checkedEmployees = {};
    for (let employee of allEmployees) {
        if (checkEmployeeIdioms(employee, idioms)) {
            checkedEmployees[employee.FirstName + ' ' + employee.LastName] = employee.Dni;
        }
    }
    return checkedEmployees;
}

function setModal(employeesBySelectedIdioms) {
    let employeesList = document.getElementById(EMPLOYEES_MODAL_BODY);
    employeesList.innerHTML = "";
    let list = document.createElement('ul');
    for (let [name, dni] of Object.entries(employeesBySelectedIdioms)) {
        let listItem = document.createElement("li");
        listItem.innerHTML = '<a href="Employee?dni=' + dni + '" >' + name + '</a>';
        list.appendChild(listItem);
    }

    employeesList.appendChild(list);
}

function checkEmployeeIdioms(employee, idioms) {
    let counter = 0;
    for (let idiom of idioms) {
        counter += employee.Languages.includes(idiom) ? 1 : 0;
    }
    return counter == idioms.length;
}

function displayAllIdioms() {
    let half = Math.ceil(languagesCount / 2);
    let index = 0;
    index = displayColumList(0, half, LEFT_COLUMN_LIST);
    index = displayColumList(index, languagesCount, RIGHT_COLUMN_LIST);
}

function displayColumList(initialIndex, lastIndex, column) {
    let index = 0;
    for (index = initialIndex; index < lastIndex; ++index) {
        let listItem = document.createElement("li");
        let employeesByLanguage = getEmployeesByIdiom(allIdioms[index]);
        let innerContent = '<p style="display: inline-block; width: 25%;">' + allIdioms[index] + ': ' + "</p>" + getButton(allIdioms[index], Object.entries(employeesByLanguage).length);
        listItem.innerHTML = innerContent;
        document.getElementById(column).appendChild(listItem);
    }

    return index;
}

function getEmployeesByIdiom(idiom) {
    let employees = {};
    for (let employee of allEmployees) {
        if (employee.Languages.includes(idiom)) {
            employees[employee.FirstName + ' ' + employee.LastName] = employee.Dni;
        }
    }
    return employees;
}

function getButton(id, employeesCount) {
    let button = '<button type="button" id="' + id + '" class="btn btn-success" onclick="changeModal(this.id)" data-toggle="modal" data-target="#employeesModal">Ver funcionarios (' + employeesCount + ') </button>';
    return button;
}

function hideAllIdioms() {
    document.getElementById(LEFT_COLUMN_LIST).innerHTML = "";
    document.getElementById(RIGHT_COLUMN_LIST).innerHTML = "";
    hide(ALL_IDIOMS_LIST_CONTAINER);
}

function changeModal(id) {
    let employeesByLanguage = getEmployeesByIdiom(id);
    setModal(employeesByLanguage);
}

function changeModalByIdiomsIntersection() {
    let idioms = getSelectedIdioms();
    setModal(getEmployeesByIdioms(idioms));
}

function getSelectedIdioms() {
    let selectedIdioms = document.getElementById(INPUT_IDIOMS).value;
    selectedIdioms = String(selectedIdioms).replaceAll('_', ' ').substring(0, selectedIdioms.length - 1);
    let idioms = selectedIdioms.split('|');
    return idioms;
}

function show(id) {
    document.getElementById(id).style.display = "block";
}

function hide(id) {
    document.getElementById(id).style.display = "none";
}

displayAllIdioms();