
async function main() {
    document.getElementById("search").addEventListener("keyup", search);
}

function matchAllTitles(activities, text) {
    var matching = new Boolean(false);
    for (var activity in activities) {
        matching = match(activities[activity].Title, text)
            || match(activities[activity].Category, text)
            || match(activities[activity].ComplexityLevel, text)
            || matchList(activities[activity].TargetAudience, text);
        if (text == "")
            show(activities[activity].Title)
        else if (matching)
            show(activities[activity].Title)
        else
            hide(activities[activity].Title)
    }
    return matching;
}

function matchList(list, word) {
    var matching = new Boolean(false);
    for (var element in list) {
        matching = match(list[element], word);
        if (matching)
            return matching;
    }
    return matching;
}

function match(word, substring) {
    return word.toLowerCase().includes(substring.toLowerCase());
}

function hide(elementId) {
    document.getElementById(elementId).style.display = "none";
}

function show(elementId) {
    document.getElementById(elementId).style.display = "block";
}

main();