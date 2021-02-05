$(function () {
    // загрузка списка книг
    loadAuthorsData();

    // обновление списка авторов
    $("#updateBtn").click(loadAuthorsData);

    // обработчики открытия модальных окон
    $("#editAuthorModal").on("show.bs.modal", editAuthorModalLoad);

    // обработчики отправки форм
    $("#addAuthorForm").on("submit", addAuthorFormSubmit);
    $("#editAuthorForm").on("submit", editAuthorFormSubmit);
});

// загрузка информации о авторах
function loadAuthorsData() {
    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/authors",
        success: (response) => showAuthorsData(response),
        error: () => $("#authorsTableBody")
            .html("<tr class='text-danger'><td colspan='42'>Ошибка при загрузке данных. \
Убедитесь, что WebApi проект запущен по адресу <b>http://localhost:4242</b>.</td></tr>"),
        beforeSend: () => $("#authorsTableBody").html("<tr class='text-success'><td colspan='42'><b>Загрузка...</b></td></tr>")
    });
}

// вывод данных
function showAuthorsData(authors) {
    const container = $("#authorsTableBody");
    container.html("");

    authors.forEach(function (elem) {
        container.append(`<tr><td>${elem.id}</td><td>${elem.surnameNP}</td>
<td class="fit"><a data-toggle="modal" data-target="#editAuthorModal" data-id="${elem.id}" href="#"><i class="fas fa-edit"></i></a></td></tr>`)
    });
}

// обработчик открытия модального окна редактирования автора
function editAuthorModalLoad(event) {
    // очистка формы
    $("#editAuthorForm").trigger("reset");

    // получение id из специального аттрибута кнопки
    let id = $(event.relatedTarget).data("id");

    if (id !== undefined) {
        // получение информации о авторе и вывод в форму
        $.getJSON("http://localhost:4242/api/authors/" + id, function (author) {
            // заполнение полей формы
            for (let i in author) {
                $('#editAuthorForm [name="' + i + '"]')
                    .val(author[i])
                    .change();
            }
        });
    }
}

// обработчики отправки форм
function addAuthorFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "POST",
        url: "http://localhost:4242/api/authors",
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadAuthorsData();

            // скрытие модального окна
            $("#addAuthorModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Автор <strong>"+ data.surnameNP + "</strong> успешно добавлен!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}

function editAuthorFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "PUT",
        url: "http://localhost:4242/api/authors/" + data.id,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadAuthorsData();

            // скрытие модального окна
            $("#editAuthorModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Автор <strong>\""+ data.surnameNP + "\"</strong> с идентификатором <strong>" + data.id + "</strong> успешно обновлен!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}