using LibraryCatalog.Models;

namespace LibraryCatalog.Services;

public interface IMemberService
{
    public Task<List<Member>> GetAllAsync();
    public Task<Member?> GetByIdAsync(int id);
    public Task<Member> AddAsync(Member member);
    public Task<Member?> UpdateAsync(int id, Member member);
    public Task<bool> DeleteAsync(int id);
}