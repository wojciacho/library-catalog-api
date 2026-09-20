using LibraryCatalog.Models;

namespace LibraryCatalog.Services;

public interface ILoanService
{
    public Task<List<Loan>> GetAllAsync();
    public Task<Loan?> GetByIdAsync(int id);
    public Task<bool> DeleteAsync(int id);
    public Task<Loan?> BorrowAsync(int bookId, int memberId);
    public Task<Loan?> ReturnAsync(int id);
}