using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRMApp.Models
{
    public class ContactInquiry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        [DisplayName("Created On")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM, yyyy hh:mm tt}")]
        public DateTime CreatedAt { get; set; }

        public bool? isArchived { get; set; }

    }
}
