using Moq;
using LibraryCatalog.Services;
using LibraryCatalog.Repositories;
using LibraryCatalog.Models;
using Microsoft.Extensions.Caching.Memory;

public class MemberServiceTests
{
    private readonly MemoryCache _cache;
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly MemberService _service;
    private readonly Member _defaultMember;

    public MemberServiceTests()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
        _mockRepository = new Mock<IMemberRepository>();
        _service = new MemberService(_mockRepository.Object, _cache);
        _defaultMember = new Member { Name = "Wojciech", Email = "wojciech@learn.com" };
    }

    [Fact]
    public async Task GetByIdAsync_WhenMemberExists_ReturnsMember()
    {
        // Arrange
        _mockRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultMember);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(_defaultMember, result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenMemberDoesNotExist_ReturnsNull()

    {
        // Arrange
        _mockRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Member?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenMemberExists_ReturnsUpdatedMember()
    {
        // Arrange
        var updatedMember = new Member { Name = "Wojciech - Updated", Email = "wojciech@updated.com" };
        _mockRepository.Setup(s => s.UpdateAsync(1, _defaultMember)).ReturnsAsync(updatedMember);

        // Act
        var result = await _service.UpdateAsync(1, _defaultMember);

        // Assert
        Assert.Equal(updatedMember, result);
    }

    [Fact]
    public async Task UpdateAsync_WhenMemberDoesNotExists_ReturnsNull()
    {
        // Arrange
        var someMember = new Member { Name = "Some member", Email = "random@email.com" };
        _mockRepository.Setup(s => s.UpdateAsync(1, someMember)).ReturnsAsync((Member?)null);

        // Act
        var result = await _service.UpdateAsync(1, someMember);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedMember()
    {
        // Arrange
        var inputMember = new Member { Name = "Added member", Email = "added@member.com" };
        var addedMember = new Member { Name = "Added member", Email = "added@member.com", Id = 1 };

        _mockRepository.Setup(s => s.AddAsync(inputMember)).ReturnsAsync(addedMember);

        // Act
        var result = await _service.AddAsync(inputMember);

        // Assert
        Assert.Equal(addedMember, result);
    }

    [Fact]
    public async Task DeleteAsync_WhenMemberExists_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenMemberDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result);
    }
}