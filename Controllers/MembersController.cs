using LibraryCatalog.Models;
using LibraryCatalog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryCatalog.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _memberService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Member>> GetByIdAsync(int id)
    {
        var member = await _memberService.GetByIdAsync(id);

        if (member == null)
        {
            return NotFound();
        }

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<Member>> AddAsync(Member member)
    {
        var newMember = await _memberService.AddAsync(member);
        return Ok(newMember);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Member>> UpdateAsync(int id, Member member)
    {
        var updatedMember = await _memberService.UpdateAsync(id, member);

        if (updatedMember == null)
        {
            return NotFound();
        }

        return Ok(updatedMember);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Member>> DeleteAsync(int id)
    {
        var isMemberDeleted = await _memberService.DeleteAsync(id);

        if (isMemberDeleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }
}
