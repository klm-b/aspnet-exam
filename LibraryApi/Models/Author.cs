using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string SurnameNP { get; set; }

        [JsonIgnore]
        public List<Book> Books { get; set; }
    }
}
