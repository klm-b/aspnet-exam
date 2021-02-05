function loadQuery(queryNumber) {
    const container = $("#containerTable");

    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/queries/" + queryNumber,
        success: (response) => container.html(jsonArrayToTableBody(response)),
        error: () => container.html(errorRow),
        beforeSend: () => container.html(loadingRow)
    });
}