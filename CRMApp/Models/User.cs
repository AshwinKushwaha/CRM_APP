using System.ComponentModel.DataAnnotations;

namespace CRMApp.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
