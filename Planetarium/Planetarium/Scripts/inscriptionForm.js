let currentDate = new Date();
let month = currentDate.getMonth() + 1;
let currentDay = currentDate.getDate();
let day = currentDay / 10 >= 1 ? '' : '0';
day += currentDay;
document.getElementById("DateOfBirth").setAttribute("max", currentDate.getFullYear() + "-" + month + "-" + day);
document.getElementById("DateOfBirth").setAttribute("class", "form-control");