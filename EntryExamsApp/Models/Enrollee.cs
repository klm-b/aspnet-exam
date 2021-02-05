using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models
{
    public class Enrollee : Person
    {
        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Год рождения")]
        [Range(1, int.MaxValue)]
        public int BirthYear { get; set; }

        [Required]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }

        public List<Exam> Exams { get; set; }
    }
}
