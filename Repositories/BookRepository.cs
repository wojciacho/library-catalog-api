using LibraryCatalog.Data;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Repositories;

public class BookRepository : IBookRepository
{

    private readonly LibraryCatalogDbContext _context;

    public BookRepository(LibraryCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var foundBook = await _context.Books.FindAsync(id);

        if (foundBook == null)
        {
            return false;
        }

        _context.Books.Remove(foundBook);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books.AsNoTracking().ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var bookById = await _context.Books.AsNoTracking().Where(b => b.Id == id).FirstOrDefaultAsync();

        if (bookById == null)
        {
            return null;
        }

        return bookById;
    }

    public async Task<Book?> UpdateAsync(int id, Book book)
    {
        var foundBook = await _context.Books.FindAsync(id);

        if (foundBook == null)
        {
            return null;
        }

        foundBook.Author = book.Author;
        foundBook.Title = book.Title;
        foundBook.IsAvailable = book.IsAvailable;

        await _context.SaveChangesAsync();
        return foundBook;
    }
}
