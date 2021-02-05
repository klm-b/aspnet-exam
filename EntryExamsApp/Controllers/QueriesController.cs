using EntryExamsApp.Models.Data;
using EntryExamsApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Controllers
{
    public class QueriesController : Controller
    {
        private readonly EntryExamsDbContext _context;
        public QueriesController(EntryExamsDbContext context)
        {
            _context = context;
        }

        // Запрос 1
        // Выбирает из таблицы АБИТУРИЕНТЫ информацию об абитуриентах, фамилия которых начинается с заданной буквы
        public IActionResult Query1(string startsWith)
        {
            var result = startsWith != null 
                ? _context.Enrollees.Where(e => e.Surname.StartsWith(startsWith)) 
                : _context.Enrollees;

            ViewBag.StartsWith = startsWith;
            return View(result);
        }

        // Запрос 2
        // Выбирает из таблицы ЭКЗАМЕНАТОРЫ информацию об экзаменаторах, 
        // для которых установлен размер оплаты за прием одного экзамена в заданном интервале
        public IActionResult Query2(int priceMin, int priceMax)
        {
            var result = priceMin == 0 && priceMax == 0
                ? _context.Examiners
                : _context.Examiners.Where(e => e.ExamPrice >= priceMin && e.ExamPrice <= priceMax);

            ViewBag.MinPrice = priceMin;
            ViewBag.MaxPrice = priceMax;
            return View(result);
        }

        // Запрос 3
        // Выбирает из таблицы ЭКЗАМЕНЫ информацию об экзаменах, принятых заданным преподавателем
        public IActionResult Query3(int examinerId)
        {
            var exams = _context.Exams.Include(e => e.Discipline).Include(e => e.Enrollee).Include(e => e.Examiner);

            var result = examinerId == 0
                ? exams
                : exams.Where(e => e.ExaminerId == examinerId);
            
            ViewBag.Examiners = new SelectList(_context.Examiners
                .Select(e => new { e.Id, FullName = $"{e.Surname} {e.Name} {e.Patronymic}" }).ToList()
                .Append(new { Id = 0, FullName = $"Все экзаменаторы" }),
                "Id", "FullName", examinerId);

            return View(result);
        }

        // Запрос 4
        // Выбирает из таблицы ЭКЗАМЕНАТОРЫ информацию об экзаменаторе с заданными фамилией, именем, отчеством.
        // Конкретные фамилия, имя и отчество вводятся при выполнении запроса
        public IActionResult Query4(string surname, string name, string patronymic)
        {

            var result = surname == null && name == null && patronymic == null
                ? _context.Examiners
                : _context.Examiners.Where(e => (surname == null || e.Surname == surname)
                && (name == null || e.Name == name)
                && (patronymic == null || e.Patronymic == patronymic));

            ViewBag.Surname = surname;
            ViewBag.Name = name;
            ViewBag.Patronymic = patronymic;
            return View(result);
        }

        // Запрос 5
        // Выбирает из таблиц АБИТУРИЕНТЫ, ЭКЗАМЕНАТОРЫ и ЭКЗАМЕНЫ информацию обо всех экзаменах
        // (ФИО абитуриента, ФИО экзаменатора, Наименование дисциплины, Дата сдачи экзамена, Оценка) 
        // в некоторый заданный интервал времени.Нижняя и верхняя границы интервала задаются при выполнении запроса
        public IActionResult Query5(DateTime? dateFrom, DateTime? dateTo)
        {
            var exams = _context.Exams
                .Include(e => e.Discipline)
                .Include(e => e.Enrollee)
                .Include(e => e.Examiner);

            var result = dateFrom == null && dateTo == null
                ? exams
                : exams.Where(e => (dateFrom == null || e.Date > dateFrom)
                && (dateTo == null || e.Date < dateTo));

            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            return View(result);
        }

        // Запрос 6
        // Вычисляет для каждого экзамена размер налога(Налог= Размер оплаты*13%) 
        // и зарплаты экзаменатора(Зарплата= Размер оплаты - Налог). Сортировка по полю Дата сдачи экзамена
        public IActionResult Query6()
        {
            var exams = _context.Exams
                .Include(e => e.Discipline)
                .Include(e => e.Enrollee)
                .Include(e => e.Examiner);

            var result = exams.Select(e => new Query6ViewModel
            {
                Enrollee = $"{e.Enrollee.Surname} {e.Enrollee.Name} {e.Enrollee.Patronymic}",
                Examiner = $"{e.Examiner.Surname} {e.Examiner.Name} {e.Examiner.Patronymic}",
                Mark = e.Mark,
                Discipline = e.Discipline.Name,
                Tax = e.Examiner.ExamPrice * 0.13,
                Salary = e.Examiner.ExamPrice - e.Examiner.ExamPrice * 0.13
            }).OrderBy(p => p.Mark);


            return View(result);
        }

        // Запрос 7
        // Выполняет группировку по полю Год рождения в таблице АБИТУРИЕНТЫ.
        // Для каждой группы определяет количество абитуриентов
        public IActionResult Query7()
        {
            var result = _context.Enrollees
                .GroupBy(s => s.BirthYear)
                .Select(g => new Query7ViewModel()
            {
                BirthYear = g.Key,
                Count = g.Count()
            });

            return View(result);
        }

        // Запрос 8
        // Выполняет группировку по полю Наименование дисциплины в таблице ЭКЗАМЕНЫ.
        // Для каждой дисциплины вычисляет среднее значения по полю Оценка
        public IActionResult Query8()
        {
            var exams = _context.Exams
                .Include(e => e.Discipline)
                .Include(e => e.Enrollee)
                .Include(e => e.Examiner).ToList();

            var result = exams
                .GroupBy(s => s.Discipline)
                .Select(g => new Query8ViewModel()
                {
                    Discipline = g.Key.Name,
                    AvgMark = g.Average(p => p.Mark),
                    ExamsCount = g.Count()
                });

            return View(result);
        }
    }
}
