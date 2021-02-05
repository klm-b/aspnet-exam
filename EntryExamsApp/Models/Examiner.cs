using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models
{
    public class Examiner : Person
    {
        [Display(Name = "Цена приема")]
        [Range(1, int.MaxValue)]
        public int ExamPrice { get; set; }

        public List<Exam> Exams { get; set; }
    }
}
