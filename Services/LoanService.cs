using LibraryCatalog.Data;
using LibraryCatalog.Models;
using LibraryCatalog.Repositories;

namespace LibraryCatalog.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly LibraryCatalogDbContext _context;

    public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, LibraryCatalogDbContext context)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _context = context;
    }

    public async Task<Loan?> BorrowAsync(int bookId, int memberId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book == null || !book.IsAvailable)
        {
            return null;
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {

            book.IsAvailable = false;
            await _bookRepository.UpdateAsync(bookId, book);

            var loanObject = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                LoanDate = DateTime.UtcNow

            };

            var addedLoan = await _loanRepository.AddAsync(loanObject);
            await transaction.CommitAsync();
            return addedLoan;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _loanRepository.DeleteAsync(id);
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _loanRepository.GetByIdAsync(id);
    }

    public async Task<Loan?> ReturnAsync(int id)
    {

        var loan = await _loanRepository.GetByIdAsync(id);

        if (loan == null)
        {
            return null;
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            loan.ReturnDate = DateTime.UtcNow;
            await _loanRepository.UpdateAsync(id, loan);

            var book = await _bookRepository.GetByIdAsync(loan.BookId);

            if (book == null)
            {
                return null;
            }

            book.IsAvailable = true;

            await _bookRepository.UpdateAsync(loan.BookId, book);

            await transaction.CommitAsync();
            return loan;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
}