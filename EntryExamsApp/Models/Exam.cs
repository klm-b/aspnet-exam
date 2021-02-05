using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models
{
    public class Exam
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Оценка")]
        [Range(1, 5)]
        public int Mark { get; set; }

        [Required]
        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Абитуриент")]
        public Enrollee Enrollee { get; set; }

        [Display(Name = "Абитуриент")]
        public int EnrolleeId { get; set; }

        [Display(Name = "Экзаменатор")]
        public Examiner Examiner { get; set; }

        [Display(Name = "Экзаменатор")]
        public int ExaminerId { get; set; }

        [Display(Name = "Дисциплина")]
        public Discipline Discipline { get; set; }

        [Display(Name = "Дисциплина")]
        public int DisciplineId { get; set; }
    }
}
