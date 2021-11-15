let currentDate = new Date();
let month = currentDate.getMonth() + 1;
let currentDay = currentDate.getDate();
let day = currentDay / 10 >= 1 ? '' : '0';
day += currentDay;
document.getElementById("Date").setAttribute("min", currentDate.getFullYear() + "-" + month + "-" + day);
document.getElementById("Date").setAttribute("class", "form-control");