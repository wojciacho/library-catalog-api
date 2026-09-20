using LibraryCatalog.Models;
using LibraryCatalog.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryCatalog.Services;

public class MemberService : IMemberService
{

    private readonly IMemberRepository _memberRepository;
    private readonly IMemoryCache _cache;
    private static string MembersCacheKey => "members_all";
    private static string MemberCacheKey(int id) => $"member_{id}";

    public MemberService(IMemberRepository memberRepository, IMemoryCache cache)
    {
        _memberRepository = memberRepository;
        _cache = cache;
    }

    public async Task<Member> AddAsync(Member member)
    {
        var addedMember = await _memberRepository.AddAsync(member);
        _cache.Remove(MembersCacheKey);
        return addedMember;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deletedMember = await _memberRepository.DeleteAsync(id);
        _cache.Remove(MembersCacheKey);
        _cache.Remove(MemberCacheKey(id));
        return deletedMember;

    }

    public async Task<List<Member>> GetAllAsync()
    {
        if (_cache.TryGetValue(MembersCacheKey, out List<Member>? cached))
        {
            return cached!;
        }

        var allMembers = await _memberRepository.GetAllAsync();
        _cache.Set(MembersCacheKey, allMembers, TimeSpan.FromMinutes(1));
        return allMembers;
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        if (_cache.TryGetValue(MemberCacheKey(id), out Member? cached))
        {
            return cached!;
        }

        var memberById = await _memberRepository.GetByIdAsync(id);
        _cache.Set(MemberCacheKey(id), memberById, TimeSpan.FromMinutes(1));
        return memberById;
    }

    public async Task<Member?> UpdateAsync(int id, Member member)
    {
        var updatedMember = await _memberRepository.UpdateAsync(id, member);
        _cache.Remove(MembersCacheKey);
        _cache.Remove(MemberCacheKey(id));
        return updatedMember;
    }
}