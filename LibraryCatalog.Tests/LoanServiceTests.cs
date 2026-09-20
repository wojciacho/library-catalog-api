using Moq;
using LibraryCatalog.Services;
using LibraryCatalog.Repositories;
using LibraryCatalog.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.Data;

public class LoanServiceTests
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");
    private readonly DbContextOptions<LibraryCatalogDbContext> _options;
    private readonly LibraryCatalogDbContext _context;
    private readonly Mock<ILoanRepository> _mockLoanRepository;
    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly LoanService _service;
    private readonly Book _defaultBook;
    private readonly Book _defaultNotAvailableBook;
    private readonly Loan _defaultLoan;

    public LoanServiceTests()
    {

        _connection.Open();
        _options = new DbContextOptionsBuilder<LibraryCatalogDbContext>().UseSqlite(_connection).Options;
        _context = new LibraryCatalogDbContext(_options);
        _mockLoanRepository = new Mock<ILoanRepository>();
        _mockBookRepository = new Mock<IBookRepository>();
        _service = new LoanService(_mockLoanRepository.Object, _mockBookRepository.Object, _context);
        _defaultBook = new Book { Title = "Learn C#", Author = "Wojciech", IsAvailable = true };
        _defaultNotAvailableBook = new Book { Title = "Learn C#", Author = "Wojciech", IsAvailable = false };
        _defaultLoan = new Loan { Id = 1, BookId = 1, MemberId = 1, LoanDate = DateTime.UtcNow, Book = _defaultBook, ReturnDate = null };
    }

    [Fact]
    public async Task GetByIdAsync_WhenLoanExists_ReturnsLoan()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultLoan);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(_defaultLoan, result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenLoanDoesNotExists_ReturnsNull()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Loan?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenLoanExists_ReturnsTrue()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenLoanDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task BorrowAsync_WhenBookIsAvailable_ReturnsLoan()
    {
        // Arrange
        _mockBookRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultBook);
        _mockLoanRepository.Setup(s => s.AddAsync(It.IsAny<Loan>())).ReturnsAsync(_defaultLoan);

        // Act
        var result = await _service.BorrowAsync(1, 1);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task BorrowAsync_WhenBookDoesNotExist_ReturnsNull()
    {
        // Arrange
        _mockBookRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Book?)null);

        // Act
        var result = await _service.BorrowAsync(1, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task BorrowAsync_WhenBookIsNotAvailable_ReturnsNull()
    {
        // Arrange
        _mockBookRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultNotAvailableBook);

        // Act
        var result = await _service.BorrowAsync(1, 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ReturnAsync_WhenLoanExistsAndNotReturned_ReturnsLoan()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultLoan);
        _mockBookRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultNotAvailableBook);

        // Act
        var result = await _service.ReturnAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.ReturnDate);
        Assert.True(_defaultNotAvailableBook.IsAvailable);
    }

    [Fact]
    public async Task ReturnAsync_WhenLoanDoesNotExist_ReturnsNull()
    {
        // Arrange
        _mockLoanRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Loan?)null);

        // Act
        var result = await _service.ReturnAsync(1);

        // Assert
        Assert.Null(result);
    }
}