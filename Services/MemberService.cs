using LibraryCatalog.Models;
using LibraryCatalog.Repositories;

namespace LibraryCatalog.Services;

public class MemberService : IMemberService
{

    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Member> AddAsync(Member member)
    {
        return await _memberRepository.AddAsync(member);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _memberRepository.DeleteAsync(id);
    }

    public async Task<List<Member>> GetAllAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task<Member?> UpdateAsync(int id, Member member)
    {
        return await _memberRepository.UpdateAsync(id, member);
    }
}