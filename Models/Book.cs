using System;
using System.ComponentModel.DataAnnotations;

namespace BookDB.Models
{

    public class Book 
    {
        [Key]
        public string ID_Book { get; set; }
        public string Name_Book { get; set; }
        public string Name_Author { get; set; }
        public string Price_Book { get; set; }
    }
}
