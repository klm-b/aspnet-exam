$(function () {
    // загрузка списка книг
    loadCategoriesData();

    // обновление списка авторов
    $("#updateBtn").click(loadCategoriesData);

    // обработчики открытия модальных окон
    $("#editCategoryModal").on("show.bs.modal", editCategoryModalLoad);

    // обработчики отправки форм
    $("#addCategoryForm").on("submit", addCategoryFormSubmit);
    $("#editCategoryForm").on("submit", editCategoryFormSubmit);
});

// загрузка информации о авторах
function loadCategoriesData() {
    $.ajax({
        type: "GET",
        url: "http://localhost:4242/api/categories",
        success: (response) => showCategoriesData(response),
        error: () => $("#categoriesTableBody")
            .html("<tr class='text-danger'><td colspan='42'>Ошибка при загрузке данных. \
Убедитесь, что WebApi проект запущен по адресу <b>http://localhost:4242</b>.</td></tr>"),
        beforeSend: () => $("#categoriesTableBody").html("<tr class='text-success'><td colspan='42'><b>Загрузка...</b></td></tr>")
    });
}

// вывод данных
function showCategoriesData(categories) {
    const container = $("#categoriesTableBody");
    container.html("");

    categories.forEach(function (elem) {
        container.append(`<tr><td>${elem.id}</td><td>${elem.name}</td>
<td class="fit"><a data-toggle="modal" data-target="#editCategoryModal" data-id="${elem.id}" href="#"><i class="fas fa-edit"></i></a></td></tr>`)
    });
}

// обработчик открытия модального окна редактирования категории
function editCategoryModalLoad(event) {
    // очистка формы
    $("#editCategoryForm").trigger("reset");

    // получение id из специального аттрибута кнопки
    let id = $(event.relatedTarget).data("id");

    if (id !== undefined) {
        // получение информации о авторе и вывод в форму
        $.getJSON("http://localhost:4242/api/categories/" + id, function (caregory) {
            // заполнение полей формы
            for (let i in caregory) {
                $('#editCategoryForm [name="' + i + '"]')
                    .val(caregory[i])
                    .change();
            }
        });
    }
}

// обработчики отправки форм
function addCategoryFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "POST",
        url: "http://localhost:4242/api/categories",
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadCategoriesData();

            // скрытие модального окна
            $("#addCategoryModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Категория <strong>"+ data.name + "</strong> успешно добавлена!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}

function editCategoryFormSubmit(e) {
    e.preventDefault();
    const form = $(this);
    const data = getFormData(form);

    $.ajax({
        method: "PUT",
        url: "http://localhost:4242/api/categories/" + data.id,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
    })
        .done(function () {
            // перезагрузка данных
            loadCategoriesData();

            // скрытие модального окна
            $("#editCategoryModal").modal('hide');

            // вывод сообщения
            $("#infoContainer").html("<div class='alert alert-success alert-dismissible fade show' role='alert'>\
                Категория <strong>\""+ data.name + "\"</strong> с идентификатором <strong>" + data.id + "</strong> успешно обновлена!\
                <button type='button' class='close' data-dismiss='alert' aria-label='Close'>\
                    <span aria-hidden='true'>&times;</span>\
                </button>\
            </div>");

            // сброс формы
            form.trigger('reset');
        });
}