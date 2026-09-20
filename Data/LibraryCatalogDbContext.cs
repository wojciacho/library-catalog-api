using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Data;

public class LibraryCatalogDbContext : DbContext
{
    public LibraryCatalogDbContext(DbContextOptions<LibraryCatalogDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Loan> Loans { get; set; }
}