using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Data
{
    public static class LibrarySeedExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Authors
            int authorIds = 1;
            var autorFaker = new Faker<Author>("ru")
                .CustomInstantiator(f => new Author() { Id = authorIds++ })
                .RuleFor(o => o.SurnameNP, f => $"{f.Name.LastName()} {f.Name.FirstName().First()}. {f.Name.FirstName().First()}.");


            var authors = autorFaker.Generate(19);
            authors.Add(new Author { Id = authorIds++, SurnameNP = "Абрамян М.Э." });

            modelBuilder.Entity<Author>().HasData(authors);


            // Categories
            var categories = new List<Category> {
                new Category { Id = 1, Name = "учебник"},
                new Category { Id = 2, Name = "задачник"},
                new Category { Id = 3, Name = "газета"},
                new Category { Id = 4, Name = "сборник"},
                new Category { Id = 5, Name = "книга"}
            };

            modelBuilder.Entity<Category>().HasData(categories);


            // Books
            int bookIds = 1;
            var bookFaker = new Faker<Book>("ru")
                .CustomInstantiator(f => new Book() { Id = bookIds++ })
                .RuleFor(o => o.Title, f => f.Random.ArrayElement(_titles))
                .RuleFor(o => o.Year, f => f.Random.Number(1990, 2020))
                .RuleFor(o => o.Price, f => f.Random.Number(2000, 30000))
                .RuleFor(o => o.Amount, f => f.Random.Number(1, 20))
                .RuleFor(o => o.AuthorId, f => f.Random.ListItem(authors).Id)
                .RuleFor(o => o.CategoryId, f => f.Random.ListItem(categories).Id);

            var books = bookFaker.Generate(40);
            books.Add(new Book
            {
                Id = bookIds++,
                Title = "Android для разработчиков",
                Amount = 12,
                AuthorId = 2,
                Price = 12000,
                Year = 2000,
                CategoryId = 1
            });
            books.Add(new Book
            {
                Id = bookIds++,
                Title = "LINQ pocket reference",
                Amount = 42,
                AuthorId = 3,
                Price = 14000,
                Year = 2010,
                CategoryId = 2
            });

            modelBuilder.Entity<Book>().HasData(books);
        }

        private static string[] _titles =
        {
            "Совершенный код", "Алгоритмы: построение и анализ", "Искусство программирования", "Структура и интерпретация компьютерных программ", 
            "Мифический человеко-месяц", "Java. Эффективное программирование", "Программист-прагматик. Путь от подмастерья к мастеру", 
            "Чистый код. Создание, анализ и рефакторинг", "Язык программирования Си", "Паттерны проектирования", "Шаблоны корпоративных приложений", 
            "Refactoring", "The Clean Coder: A Code of Conduct for Professional Programmers", "Современное проектирование на С++: шаблоны проектирования", 
            "Cracking the Coding Interview", "Design Patterns", "Язык программирования C++. Базовый курс", "Идеальный программист. Как стать профессионалом разработки ПО", 
            "Предметно- ориентированное проектирование", "Код: тайный язык информатики", "Изучаем Java", "Алгоритмы на Java", "Философия Java", "Java Concurrency in Practice", 
            "Типы в языках программирования", "Peopleware: Productive Projects and Teams", "JavaScript: сильные стороны", "Java: полное руководство ", 
            "Domain Modeling Made Functional: Domain-Driven Design and F#", "Domain-Driven Design Distilled", 
            "Soft Skills: The Software Developer's Life Manual", "Code: The Hidden Language of Computer Hardware and Software", 
            "Эффективный и современный С++. 42 рекомендации по использованию C++11 и C++14", "Рефакторинг кода на JavaScript: улучшение проекта существующего кода", 
            "Алгоритмы + структуры данных = программы", "Windows via C/C++, fifth edition", "Чистая архитектура. Искусство разработки программного обеспечения", 
            "Кодеры за работой: размышления о ремесле программиста ", "Объектно- ориентированное программирование в С++", "Лёгкий способ выучить Python", 
            "JavaScript. Подробное руководство", "Programming Pearls", "Coders at Work", "The Unix Programming Environment", "Изучаем программирование на JavaScript", 
            "Professional ASP.NET MVC 5", "Эффективная работа с унаследованным кодом", "Дизайн и эволюция C++", "Computer science", "Язык программирования C++"
        };
    }
}
