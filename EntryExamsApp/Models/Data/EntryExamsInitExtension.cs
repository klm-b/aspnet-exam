using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace EntryExamsApp.Models.Data
{
    public static class EntryExamsInitExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // Enrollees
            int ids = 1;
            var enrolleeFaker = new Faker<Enrollee>("ru")
                .RuleFor(o => o.Id, f => ids++)
                .RuleFor(o => o.Surname, f => f.Name.LastName(Bogus.DataSets.Name.Gender.Male))
                .RuleFor(o => o.Name, f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Male))
                .RuleFor(o => o.Patronymic, f => f.Random.ListItem(_patronymics))
                .RuleFor(o => o.Photo, f => $"photo_{f.Random.Number(1, 50)}.png")
                .RuleFor(o => o.Address, f => f.Address.FullAddress())
                .RuleFor(o => o.BirthYear, f => f.Random.Number(1997, 2003))
                .RuleFor(o => o.Passport, f => f.Random.Replace("??######??").ToUpper());


            var enrollees = enrolleeFaker.Generate(20);
            modelBuilder.Entity<Enrollee>().HasData(enrollees);


            // Examiners
            var examinerFaker = new Faker<Examiner>("ru")
                .RuleFor(o => o.Id, f => ids++)
                .RuleFor(o => o.Surname, f => f.Name.LastName(Bogus.DataSets.Name.Gender.Male))
                .RuleFor(o => o.Name, f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Male))
                .RuleFor(o => o.Patronymic, f => f.Random.ListItem(_patronymics))
                .RuleFor(o => o.Photo, f => $"photo_{f.Random.Number(1, 50)}.png")
                .RuleFor(o => o.ExamPrice, f => f.Random.Number(1000, 10000));


            var examiners = examinerFaker.Generate(5);
            modelBuilder.Entity<Examiner>().HasData(examiners);


            // Disciplines
            var disciplines = new List<Discipline> {
                new Discipline { Id = 1, Name = "математика"},
                new Discipline { Id = 2, Name = "история"},
                new Discipline { Id = 3, Name = "химия"},
                new Discipline { Id = 4, Name = "физика"},
                new Discipline { Id = 5, Name = "английский язык"}
            };

            modelBuilder.Entity<Discipline>().HasData(disciplines);

            // Exams
            ids = 1;
            var examFaker = new Faker<Exam>("ru")
                .RuleFor(o => o.Id, f => ids++)
                .RuleFor(o => o.Mark, f => f.Random.Number(1, 5))
                .RuleFor(o => o.Date, f => f.Date.Past())
                .RuleFor(o => o.EnrolleeId, f => f.Random.ListItem(enrollees).Id)
                .RuleFor(o => o.ExaminerId, f => f.Random.ListItem(examiners).Id)
                .RuleFor(o => o.DisciplineId, f => f.Random.ListItem(disciplines).Id);

            var exams = examFaker.Generate(50);
            modelBuilder.Entity<Exam>().HasData(exams);
        }

        private static string[] _patronymics =
        {
            "Александрович", "Алексеевич", "Анатольевич", "Андреевич", "Антонович", "Аркадьевич", 
            "Арсеньевич", "Артемович", "Афанасьевич", "Богданович", "Борисович", "Вадимович", 
            "Валентинович", "Валериевич", "Васильевич", "Викторович", "Витальевич", "Владимирович", 
            "Всеволодович", "Вячеславович", "Гаврилович", "Геннадиевич", "Георгиевич", "Глебович", 
            "Григорьевич", "Давыдович", "Данилович", "Денисович", "Дмитриевич", "Евгеньевич", "Егорович", 
            "Емельянович", "Ефимович", "Иванович", "Игоревич", "Ильич", "Иосифович", "Кириллович", 
            "Константинович", "Корнеевич", "Леонидович", "Львович", "Макарович", "Максимович", "Маркович", 
            "Матвеевич", "Митрофанович", "Михайлович", "Назарович", "Наумович", "Николаевич", "Олегович", 
            "Павлович", "Петрович", "Платонович", "Робертович", "Родионович", "Романович", "Савельевич", 
            "Семенович", "Сергеевич", "Станиславович", "Степанович", "Тарасович", "Тимофеевич", "Тихонович", 
            "Федорович", "Феликсович", "Филиппович", "Эдуардович", "Юрьевич", "Яковлевич", "Ярославович"
        };
    }
}
