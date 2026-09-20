namespace LibraryCatalog.Models;

public class Loan
{
    public int Id { get; set; }
    public required int BookId { get; set; }
    public required int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public Book Book { get; set; } = null!;
    public Member Member { get; set; } = null!;
}