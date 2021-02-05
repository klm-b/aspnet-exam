using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models.ViewModels
{
    public class Query6ViewModel
    {
        [Display(Name = "Оценка")]
        public int Mark { get; set; }

        [Display(Name = "Дата сдачи")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Абитуриент")]
        public string Enrollee { get; set; }

        [Display(Name = "Экзаменатор")]
        public string Examiner { get; set; }

        [Display(Name = "Дисциплина")]
        public string Discipline { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Налог")]
        public double Tax { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        [Display(Name = "Зарплата экзаменатора")]
        public double Salary { get; set; }
    }
}
