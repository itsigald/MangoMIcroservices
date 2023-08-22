using System.ComponentModel.DataAnnotations;

namespace Mango.Services.EmailAPI.Models
{
    public class EmailLogger
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string? EmailFrom { get; set; }

        [MaxLength(50)]
        public string? EmailTo { get; set; }

        public string? Message { get; set; }

        public DateTime EmailSent { get; set; }
    }
}
