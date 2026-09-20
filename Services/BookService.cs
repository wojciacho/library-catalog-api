using LibraryCatalog.Models;
using LibraryCatalog.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryCatalog.Services;

public class BookService : IBookService

{
    private readonly IBookRepository _bookRepository;
    private readonly IMemoryCache _cache;
    private static string BooksCacheKey => "books_all";
    private static string BookCacheKey(int id) => $"books_{id}";

    public BookService(IBookRepository bookRepository, IMemoryCache cache)
    {
        _bookRepository = bookRepository;
        _cache = cache;
    }

    public async Task<Book> AddAsync(Book book)
    {
        var addedBook = await _bookRepository.AddAsync(book);
        _cache.Remove(BooksCacheKey);
        return addedBook;

    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedBook = await _bookRepository.DeleteAsync(id);
        _cache.Remove(BooksCacheKey);
        _cache.Remove(BookCacheKey(id));
        return deletedBook;

    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        if (_cache.TryGetValue(BookCacheKey(id), out Book? cached))
        {
            return cached!;
        }

        var bookById = await _bookRepository.GetByIdAsync(id);
        _cache.Set(BookCacheKey(id), bookById, TimeSpan.FromMinutes(1));
        return bookById;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        if (_cache.TryGetValue(BooksCacheKey, out List<Book>? cached))
        {
            return cached!;
        }

        var allBooks = await _bookRepository.GetAllAsync();
        _cache.Set(BooksCacheKey, allBooks, TimeSpan.FromMinutes(1));
        return allBooks;
    }

    public async Task<Book?> UpdateAsync(int id, Book book)
    {
        var updatedBook = await _bookRepository.UpdateAsync(id, book);
        _cache.Remove(BooksCacheKey);
        _cache.Remove(BookCacheKey(id));
        return updatedBook;
    }
}

