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
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        
        public DateTime CreatedAt { get; set; }

        [BindNever]
        public DateTime? UpdatedAt { get; set; }

        [BindNever]
        public ICollection<CustomerContact>? Contacts { get; set; }

    }
}
