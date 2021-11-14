const INPUT_TOPIC_STRING = "inputTopicString";
const INPUT_CATEGORY_STRING = "inputCategoryString";
const CATEGORIES_CHART = "categoriesChart";
const TOPICS_CHART = "topicsChart";

let categoriesData = createChart(categoriesRank, "Ranking de Categorías", CATEGORIES_CHART);
let topicsData = createChart(topicsRank, "Ranking de Tópicos", TOPICS_CHART);

function createChart(elementsRank, title, idChartContainer) {
    let items = Object.keys(elementsRank).map(function (key) {
        return [key, elementsRank[key]];
    });
    let chart = anychart.bar();
    let data = anychart.data.set(items);
    chart.labels(true);
    chart.bar(data);
    chart.title(title);
    chart.container(idChartContainer);
    chart.draw();
    return data;
}

function updateChart(inputElementString, idChartContainer, rankingType) {
    let selectedElements = document.getElementById(inputElementString).value;

    selectedElements = String(selectedElements).replaceAll('_', ' ');
    let chartData = (rankingType == 1) ? categoriesData : topicsData;
    let elementsRank = (rankingType == 1) ? categoriesRank : topicsRank;

    selectedElements = sortElements(selectedElements, elementsRank).substring(0, selectedElements.length - 1);
    removeAll(chartData, Object.keys(elementsRank).length);

    let words = (document.getElementById(inputElementString).value == "" && rankingType == 1) ? Object.keys(categoriesRank).map(function (key) {
        return key;
    }) : selectedElements.split('|');

    let counter = 0;

    for (let word of words) {
        if (word != "") {
            chartData.insert({ x: word, value: elementsRank[word] }, counter++);
        }
    }

    if (chartData.getRowsCount() == 0) {
        document.getElementById(idChartContainer).setAttribute("style", "height: 500px; width: 100%; display: none;");
    } else {
        document.getElementById(idChartContainer).setAttribute("style", "height: 500px; width: 100%; display: block;");
    }
}

function removeAll(chartData, length) {
    for (let element = 0; element < length; ++element) {
        chartData.remove(0);
    }
}

function sortElements(selectedElements, rankType) {
    let rankedElements = "";
    for (let element in rankType) {
        if (selectedElements.search(element) != -1) {
            rankedElements += element + "|";
        }
    }
    return rankedElements;
}

function updateTopics(value) {
    let inputCategoryString = document.getElementById(INPUT_CATEGORY_STRING).value.replaceAll('_', ' ');

    if (value != '' && !inputCategoryString.includes(value)) {
        $.ajax({
            type: 'POST',
            url: $("#controllerURL").data("request-url"),
            dataType: 'json',

            data: { category: value },

            success: function (topics) {
                $.each(topics, function (i, topic) {
                    if (topicsRank[topic.Value]) {
                        $("#topicSelect").append('<option class="' + String(value.replaceAll(" ", "_")) + '" value=' + topic.Value.replace(" ", "_") + '_(' + String(value.replaceAll(" ", "_")) + ')' + '>' + topic.Text + ' (' + value + ') ' + '</option>');
                    }
                });
            },
            error: function (ex) {
                alert('Fallo en la recuperación de tópicos' + ex);
            }

        });
        return false;
    }
}


function deleteTopics() {
    let selectedElements = document.getElementById(INPUT_CATEGORY_STRING).value.replaceAll("_", " ");
    let containerChildren = document.getElementById('topicsContainer').children;

    for (const [category, value] of Object.entries(categoriesRank)) {
        if (!selectedElements.includes(category)) {
            document.querySelectorAll("." + String(category.replaceAll(" ", "_"))).forEach(e => e.remove());
            let count = containerChildren.length;
            let index = 0;
            while (index < count) {
                let button = containerChildren[index];
                if (button && button.tagName == "A") {
                    if (button.innerHTML.includes(category)) {
                        button.click()
                        --index;
                    }
                }
                ++index;
            }
        }
    }
}

function hideTopicsChart() {
    let topicString = document.getElementById(INPUT_TOPIC_STRING).value;
    if (topicString == "") {
        hide(TOPICS_CHART);
    }
}

function hide(id) {
    document.getElementById(id).style.display = "none";
}

function showTopicsChart() {
    let topicString = document.getElementById(INPUT_TOPIC_STRING).value;
    let categoryString = document.getElementById(INPUT_CATEGORY_STRING).value;

    if (topicString == "" && categoryString == "") {
        document.getElementById(TOPICS_CHART).innerHTML = "";
        topicsData = createChart(topicsRank, "Ranking de Tópicos", TOPICS_CHART);
        show(TOPICS_CHART);
    }
}

function show(id) {
    document.getElementById(id).style.display = "block";
}