using Microsoft.AspNetCore.Mvc;
using LibraryCatalog.Models;
using LibraryCatalog.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetByIdAsync(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> AddAsync(Book book)
    {
        var newBook = await _bookService.AddAsync(book);
        return Ok(newBook);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> UpdateBookAsync(int id, Book book)
    {
        var updatedBook = await _bookService.UpdateAsync(id, book);

        if (updatedBook == null)
        {
            return NotFound();
        }

        return Ok(updatedBook);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Book>> DeleteAsync(int id)
    {
        var isBookDeleted = await _bookService.DeleteAsync(id);

        if (isBookDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
};

