using System.ComponentModel.DataAnnotations;

namespace NTC.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Job { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public string Contacts { get; set; }
    }
}
