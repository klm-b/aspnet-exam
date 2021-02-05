using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models.ViewModels
{
    public class ExaminerViewModel
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

        [Display(Name = "Цена приема")]
        [Range(1, int.MaxValue)]
        public int ExamPrice { get; set; }
    }
}
