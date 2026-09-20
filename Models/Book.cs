using System.ComponentModel.DataAnnotations;

namespace LibraryCatalog.Models;

public class Book
{
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string Author { get; set; }
    public int Id { get; set; }
    public int Year { get; set; }
    public int Pages { get; set; }
    public bool IsAvailable { get; set; }
}