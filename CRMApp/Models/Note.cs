using CRMApp.Areas.Identity.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRMApp.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public string Description { get; set; }


        public string CreatedBy { get; set; }

        [DisplayName("Created on")]
        [DisplayFormat(DataFormatString = "{0:dd MMMM, yyyy hh:mm tt}")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
