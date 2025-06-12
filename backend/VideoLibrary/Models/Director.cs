using System.ComponentModel.DataAnnotations;

namespace VideoLibrary.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Bio { get; set; }


    }
}
