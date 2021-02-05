using LibraryApi.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/queries")]
    [ApiController]
    public class QueriesController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public QueriesController(LibraryDbContext context)
        {
            _db = context;
        }

        [HttpGet("{id}")]
        public ActionResult ExecQuery(int id)
        {
            IEnumerable result;

            switch (id)
            {
                case 1:
                    result = Query1();
                    break;

                case 2:
                    result = Query2("android", "учебник");
                    break;

                case 3:
                    result = Query3("LINQ", "задачник");
                    break;

                case 4:
                    result = Query4(4, 6);
                    break;

                case 5:
                    result = Query5();
                    break;

                case 6:
                    result = Query6();
                    break;

                case 7:
                    result = Query7("Абрамян М.Э.", 0.20);
                    break;

                case 8:
                    result = Query8();
                    break;

                default:
                    return NotFound();
            };

            return Ok(result);
        }

        // Запрос 1. Вывести полную информацию обо всех книгах 
        private IEnumerable Query1()
        {
            var result = _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    Author = b.Author.SurnameNP,
                    Category = b.Category.Name,
                    b.Year,
                    b.Price,
                    b.Amount
                });

            return result.ToList();
        }


        // Запрос 2. Вывести название, год издания, автора и цену учебников по Android
        private IEnumerable Query2(string str, string bookType)
        {
            var result = _db.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .ToList()
                .Where(b => b.Category.Name == bookType && b.Title.Contains(str, StringComparison.CurrentCultureIgnoreCase))
                .Select(b => new
                {
                    b.Title,
                    b.Year,
                    Author = b.Author.SurnameNP,
                    Category = b.Category.Name,
                    b.Price
                });

            return result.ToList();
        }


        // Запрос 3. Вывести название, год издания и количество задачников по LINQ
        private IEnumerable Query3(string str, string bookType)
        {
            var result = _db.Books
                .Include(b => b.Category)
                .ToList()
                .Where(b => b.Category.Name == bookType && b.Title.Contains(str, StringComparison.InvariantCultureIgnoreCase))
                .Select(b => new
                {
                    b.Title,
                    b.Year,
                    b.Amount
                });

            return result.ToList();
        }


        // Запрос 4. Вывести автора, название, категорию и стоимость для каждой книги, 
        // количество которых от 4 до 6
        private IEnumerable Query4(int min, int max)
        {
            var result = _db.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.Amount >= min && b.Amount <= max)
                .Select(b => new
                {
                    Author = b.Author.SurnameNP,
                    b.Title,
                    Category = b.Category.Name,
                    b.Amount,
                    b.Price
                });

            return result.ToList();
        }


        // Запрос 5. Вывести фамилии и инициалы авторов и количество книг этих авторов
        private IEnumerable Query5()
        {
            var result = _db.Authors.Select(a => new
            {
                a.Id,
                a.SurnameNP,
                BooksCount = a.Books.Count
            });

            return result.ToList();
        }


        // Запрос 6. Для категорий книг вывести количество, минимальную стоимость книги, 
        // среднюю стоимость книги, максимальную стоимость книги
        private IEnumerable Query6()
        {
            var result = _db.Categories
                .Include(a => a.Books)
                .Select(a => new
                {
                    a.Id,
                    Category = a.Name,
                    a.Books.Count,
                    MinPrice = a.Books.Count > 0 ? a.Books.Min(b => b.Price) : 0,
                    AvgPrice = a.Books.Count > 0 ? a.Books.Average(b => b.Price) : 0,
                    MaxPrice = a.Books.Count > 0 ? a.Books.Max(b => b.Price) : 0
                });

            return result.ToList();
        }


        // Запрос 7. Для всех книг автора Абрамян М.Э.увеличить стоимость книг на 15%
        private IEnumerable Query7(string authorToUpd, double percent)
        {
            var author = _db.Authors
                .Include(a => a.Books)
                .FirstOrDefault(a => a.SurnameNP == authorToUpd);
            if (author == null) return null;

            foreach (var book in author.Books)
            {
                book.Price += (int)(book.Price * percent);
            }

            _db.SaveChanges();

            return author.Books.ToList();
        }

        // Запрос 8. Уменьшить количество книг по C++ на 1, не допускать отрицательных значений.
        private IEnumerable Query8()
        {
            var books = _db.Books
                .ToList()
                .Where(b => b.Title.Contains("C++", StringComparison.InvariantCultureIgnoreCase));

            foreach (var book in books)
            {
                book.Amount -= book.Amount <= 1 ? 0 : 1;
            }

            _db.SaveChanges();

            return books.ToList();
        }
    }
}
