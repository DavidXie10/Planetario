class ReportGenerator {
    constructor(inputDateIds, tableId) {
        this.startInput = document.querySelector(inputDateIds[0]);
        this.endInput = document.querySelector(inputDateIds[1]);
        this.table = document.querySelector(tableId);
    }

    async generateReport(fetchUrl, tableHeaderNames) {
        fetchUrl += "?startDate=" + this.startInput.value + "&endDate=" + this.endInput.value;
        let items = await this.fetchModelsFromDatabase(fetchUrl);
        this.loadTable(items, tableHeaderNames);
    }

    loadTable(data, tableHeaderNames) {
        this.table.innerText = "";
        this.loadTableHeader(tableHeaderNames);
        this.loadTableContent(data);
    }

    loadTableHeader(tableHeaderNames) {
        let tableHead = this.table.createTHead();
        let row = tableHead.insertRow(0);
        let attributeCounter = 0;

        tableHeaderNames.forEach(name => {
            let cell = row.insertCell(attributeCounter);
            let text = document.createTextNode(name);
            cell.appendChild(text);
            cell.style.textAlign = "center";
            attributeCounter++;
        });
    }

    loadTableContent(data) {
        let tbody = this.table.createTBody();
        data.forEach(element => {
            let tr = document.createElement("tr");
            for (const [key, value] of Object.entries(element)) {
                let td = document.createElement("td");
                let result = value;
                if (key == "Income") {
                    result = '₡' + result;
                } else if (key == "StartActivityDay" || key == "LastBoughtDate") {
                    result = (new Date(parseInt(result.substr(6))));
                    result = result.toLocaleDateString("en-US");
                }
                let text = document.createTextNode(result);

                td.appendChild(text);
                td.style.textAlign = "center";
                tr.appendChild(td);

            }
            tbody.appendChild(tr);
        })
    }

    async fetchModelsFromDatabase(fetchUrl) {
        const response = await fetch(fetchUrl);
        const data = await response.json();
        return data;
    }
}