using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntryExamsApp.Models.ViewModels
{
    public class Query7ViewModel
    {
        [Display(Name = "Год рождения")]
        public int BirthYear { get; set; }

        [Display(Name = "Кол-во абитуриентов")]
        public int Count { get; set; }
    }
}
