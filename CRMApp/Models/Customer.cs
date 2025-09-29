using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace CRMApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "* Enter a valid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 10,MinimumLength = 10,ErrorMessage = "* Phone must be of 10 characters")]
        [Phone]
        public string Phone { get; set; }

        
        public DateTime CreatedAt { get; set; }

        [BindNever]
        public DateTime? UpdatedAt { get; set; }

        [BindNever]
        public ICollection<CustomerContact>? Contacts { get; set; }

    }
}
