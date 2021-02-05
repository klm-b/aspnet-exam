using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models.ViewModels
{
    public class Query8ViewModel
    {
        [Display(Name = "Дисциплина")]
        public string Discipline { get; set; }

        [DisplayFormat(DataFormatString = "{0:n1}")]
        [Display(Name = "Средняя оценка")]
        public double AvgMark { get; set; }

        [Display(Name = "Кол-во экзаменов")]
        public int ExamsCount { get; set; }
    }
}
