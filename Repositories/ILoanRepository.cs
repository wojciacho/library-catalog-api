using LibraryCatalog.Models;

namespace LibraryCatalog.Repositories;

public interface ILoanRepository
{
    public Task<List<Loan>> GetAllAsync();
    public Task<Loan?> GetByIdAsync(int id);
    public Task<Loan> AddAsync(Loan loan);
    public Task<Loan?> UpdateAsync(int id, Loan loan);
    public Task<bool> DeleteAsync(int id);
}