using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models.ViewModels
{
    public class EnrolleeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Фото")]
        public IFormFile Photo { get; set; }

        public string PhotoPath { get; set; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Год рождения")]
        [Range(1, int.MaxValue)]
        public int BirthYear { get; set; }

        [Required]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }
    }
}
