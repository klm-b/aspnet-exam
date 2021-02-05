$(function () {
    loadTrainersData();
});

// загрузка карточек залов на главную страницу
function loadTrainersData() {
    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/admin/getTrainers",
        success: (response) => parseTrainersData(response),
        error: e => console.log(e)
    });
}

// парсинг данных
function parseTrainersData(trainers) {
    const container = $("#trainersTableBody");

    trainers.forEach(function (elem) {
        container.append("<tr>\
            <td class=\"fit\">" + elem.id + "</td >\
            <td>" + elem.surnameNp + "</td >\
            <td>" + elem.passport + "</td >\
            <td>" + elem.position + "</td >\
            <td>" + elem.salary + " руб.</td >\
            <td class=\"text-center fit\">\
                <a>\
                <i class=\"fas fa-info-circle p-1\"></i>\
                </a>\
                 <a>\
                <i class=\"fas fa-edit p-1 text-success\"></i>\
                </a>\
                <a>\
                <i class=\"fas fa-trash-alt p-1 text-danger\"></i>\
                </a >\
            </td >\
            </tr>"
        );
    });
}