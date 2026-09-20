using LibraryCatalog.Data;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Repositories;

public class MemberRepository : IMemberRepository
{

    private readonly LibraryCatalogDbContext _context;

    public MemberRepository(LibraryCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Member> AddAsync(Member member)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var foundMember = await _context.Members.FindAsync(id);

        if (foundMember == null)
        {
            return false;
        }

        _context.Members.Remove(foundMember);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Member>> GetAllAsync()

    {
        return await _context.Members.AsNoTracking().ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        var memberById = await _context.Members.AsNoTracking().Where(m => m.Id == id).FirstOrDefaultAsync();

        if (memberById == null)
        {
            return null;
        }

        return memberById;
    }

    public async Task<Member?> UpdateAsync(int id, Member member)
    {
        var foundMember = await _context.Members.FindAsync(id);

        if (foundMember == null)
        {
            return null;
        }

        foundMember.Email = member.Email;
        foundMember.Name = member.Name;

        await _context.SaveChangesAsync();
        return foundMember;
    }
}