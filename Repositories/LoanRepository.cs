using LibraryCatalog.Data;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Repositories;

public class LoanRepository : ILoanRepository
{

    private readonly LibraryCatalogDbContext _context;

    public LoanRepository(LibraryCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Loan> AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var foundLoan = await _context.Loans.FindAsync(id);

        if (foundLoan == null)
        {
            return false;
        }

        _context.Loans.Remove(foundLoan);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        return await _context.Loans.AsNoTracking().Include(l => l.Book).Include(l => l.Member).ToListAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        var loanById = await _context.Loans.AsNoTracking().Include(l => l.Book).Include(l => l.Member).Where(l => l.Id == id).FirstOrDefaultAsync();

        if (loanById == null)
        {
            return null;
        }

        return loanById;
    }

    public async Task<Loan?> UpdateAsync(int id, Loan loan)
    {
        var foundLoan = await _context.Loans.FindAsync(id);

        if (foundLoan == null)
        {
            return null;
        }

        foundLoan.LoanDate = loan.LoanDate;
        foundLoan.ReturnDate = loan.ReturnDate;
        foundLoan.BookId = loan.BookId;
        foundLoan.MemberId = loan.MemberId;

        await _context.SaveChangesAsync();
        return foundLoan;
    }
}