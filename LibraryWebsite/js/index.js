function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        let parsed = parseInt(n['value']);
        if (n['value']) indexed_array[n['name']] = isNaN(parsed) ? n['value'] : parsed;
    });

    return indexed_array;
}


function jsonArrayToTableBody(json) {
    let cols = Object.keys(json[0]);

    let headerRow = cols
        .map(col => `<th>${col}</th>`)
        .join("");

    let rows = json
        .map(row => {
            let tds = cols.map(col => `<td>${row[col]}</td>`).join("");
            return `<tr>${tds}</tr>`;
        })
        .join("");

    return `
		<thead>
			<tr>${headerRow}</tr>
		<thead>
		<tbody>
			${rows}
		<tbody>`;
}

const errorRow = "<tr class='text-danger'><td colspan='42'>Ошибка при загрузке данных. \Убедитесь, что WebApi проект запущен по адресу <b>http://localhost:4242</b>.</td></tr>";
const loadingRow = "<tr class='text-success'><td colspan='42'><b>Загрузка...</b></td></tr>";