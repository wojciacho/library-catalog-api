using LibraryCatalog.Models;

namespace LibraryCatalog.Repositories;

public interface IBookRepository
{
    public Task<List<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(int id);
    public Task<Book> AddAsync(Book book);
    public Task<Book?> UpdateAsync(int id, Book book);
    public Task<bool> DeleteAsync(int id);
}
