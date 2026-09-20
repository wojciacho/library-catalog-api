using LibraryCatalog.Models;
using LibraryCatalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _loanService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Loan>> GetByIdAsync(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);

        if (loan == null)
        {
            return NotFound();
        }

        return Ok(loan);
    }

    [HttpPost]
    public async Task<ActionResult<Loan>> BorrowAsync(BorrowRequest loan)
    {

        var bookId = loan.BookId;
        var memberId = loan.MemberId;

        var borrow = await _loanService.BorrowAsync(bookId, memberId);

        if (borrow == null)
        {
            return NotFound();
        }

        return Ok(borrow);
    }

    [HttpPut("{id}/return")]
    public async Task<ActionResult<Loan>> ReturnAsync(int id)
    {
        var loan = await _loanService.ReturnAsync(id);

        if (loan == null)
        {
            return NotFound(loan);
        }

        return Ok(loan);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Loan>> DeleteAsync(int id)
    {
        var isLoanDeleted = await _loanService.DeleteAsync(id);

        if (isLoanDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}