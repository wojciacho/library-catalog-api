using System.ComponentModel.DataAnnotations;

namespace LibraryCatalog.Models;

public class Member
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Email { get; set; }
}