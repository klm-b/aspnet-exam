$(function () {
    // загрузка списка книг
    loadBooksData();

    // загрузка данных в selects
    loadSelectsData();

    // обновление списка книг
    $("#updateBtn").click(loadBooksData);

    // обработчики открытия модальных окон
    $("#editBookModal").on("show.bs.modal", editBookModalLoad);
    $("#removeBookModal").on("show.bs.modal", removeBookModalLoad);

    // обработчики отправки форм
    $("#addBookForm").on("submit", addBookFormSubmit);
    $("#editBookForm").on("submit", editBookFormSubmit);
    $("#removeBookForm").on("submit", removeBookFormSubmit);
});

// загрузка информации о книгах
function loadBooksData() {
    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/books",
        success: (response) => showBooksData(response),
        error: () => $("#booksTableBody")
            .html("<tr class='text-danger'><td colspan='42'>Ошибка при загрузке данных. \
Убедитесь, что WebApi проект запущен по адресу <b>http://localhost:4242</b>.</td></tr>"),
        beforeSend: () => $("#booksTableBody").html("<tr class='text-success'><td colspan='42'><b>Загрузка...</b></td></tr>")
    });
}

// вывод данных
function showBooksData(books) {
    const container = $("#booksTableBody");
    container.html("");

    books.forEach(function (elem) {
        container.append(`<tr><td>${elem.id}</td><td>${elem.title}</td><td>${elem.author}</td>
<td>${elem.category}</td><td>${elem.year}</td><td>${elem.price} руб.</td><td>${elem.amount}</td>
<td class="fit"><a data-toggle="modal" data-target="#editBookModal" data-id="${elem.id}" href="#"><i class="fas fa-edit"></i></a>
<a data-toggle="modal" data-target="#removeBookModal" data-id="${elem.id}" href="#"><i class="fas text-danger ml-1 fa-trash"></i></a></td></tr>`)
    });
}

// загрузка данных в selects
function loadSelectsData() {
    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/categories",
        success: (response) => $('[name="categoryId"]')
            .html(response.map((category) => `<option value='${category.id}'>${category.name}</option>`).join("\n")),
        error: (e) => console.log(e.responseText)
    });

    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/authors",
        success: (response) => $('[name="authorId"]')
            .html(response.map((author) => `<option value='${author.id}'>${author.surnameNP}</option>`).join("\n")),
        error: (e) => console.log(e.responseText)
    });
}

// обработчик открытия модального окна редактирования книги
function editBookModalLoad(event) {
    // очистка формы
    $("#editBookForm").trigger("reset");

    // получение id из специального аттрибута кнопки
    let id = $(event.relatedTarget).data("id");

    if (id !== undefined) {
        // получение информации о книге и вывод в форму
        $.getJSON("http://localhost:4242/api/books/" + id, function (book) {
            // заполнение полей формы
            for (let i in book) {
                $('#editBookForm [name="' + i + '"]')
                    .val(book[i])
                    .change();
            }
        });
    }
}

// обработчик открытия модального окна удаления книги
function removeBookModalLoad(event) {
    // очистка формы
    $("#removeBookForm").trigger("reset");

    // получение id из специального аттрибута кнопки
    let id = $(event.relatedTarget).data("id");

    if (id !== undefined) {
        // получение информации о книге и вывод
        $.getJSON("http://localhost:4242/api/books/" + id, function (book) {
            $("#removeMsg").html(`Вы уверены, что хотите удалить книгу <strong>"${book.title}"</strong> c идентификатором <strong>${book.id}</strong>?`)
            $("#remove_id").val(book.id);
        });
    }
}

// обработчики отправки форм
function addBookFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "POST",
        url: "http://localhost:4242/api/books",
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadBooksData();

            // скрытие модального окна
            $("#addBookModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Книга <strong>"+ data.title + "</strong> успешно добавлена!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}

function editBookFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "PUT",
        url: "http://localhost:4242/api/books/" + data.id,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadBooksData();

            // скрытие модального окна
            $("#editBookModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Книга <strong>\""+ data.title + "\"</strong> с идентификатором <strong>" + data.id + "</strong> успешно обновлена!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}

function removeBookFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "DELETE",
        url: "http://localhost:4242/api/books/" + data.id,
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadBooksData();

            // скрытие модального окна
            $("#removeBookModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Книга <strong>\""+ data.title + "\"</strong> с идентификатором <strong>" + data.id + "</strong> успешно удалена!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}