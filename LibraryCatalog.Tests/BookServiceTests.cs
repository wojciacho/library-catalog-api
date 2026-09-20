using Moq;
using LibraryCatalog.Services;
using LibraryCatalog.Repositories;
using LibraryCatalog.Models;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockRepository;
    private readonly BookService _service;
    private readonly Book _defaultBook;

    public BookServiceTests()
    {
        _mockRepository = new Mock<IBookRepository>();
        _service = new BookService(_mockRepository.Object);
        _defaultBook = new Book { Id = 1, Title = "Jacked", Author = "David Kushner" };
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookExists_ReturnsBook()
    {
        // Arrange
        _mockRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(_defaultBook);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Equal(_defaultBook, result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookDoesNotExist_ReturnsNull()

    {
        // Arrange
        _mockRepository.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Book?)null);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenBookExists_ReturnsUpdatedBook()
    {
        // Arrange
        var updatedBook = new Book { Title = "Jacked - Updated", Author = "David Kushner - Updated" };
        _mockRepository.Setup(s => s.UpdateAsync(1, _defaultBook)).ReturnsAsync(updatedBook);

        // Act
        var result = await _service.UpdateAsync(1, _defaultBook);

        // Assert
        Assert.Equal(updatedBook, result);
    }

    [Fact]
    public async Task UpdateAsync_WhenBookDoesNotExists_ReturnsNull()
    {
        // Arrange
        var someBook = new Book { Title = "Some random title", Author = "Some random author" };
        _mockRepository.Setup(s => s.UpdateAsync(1, someBook)).ReturnsAsync((Book?)null);

        // Act
        var result = await _service.UpdateAsync(1, someBook);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedBook()
    {
        // Arrange
        var inputBook = new Book { Title = "Added book", Author = "Added author", Year = 1234, Pages = 12, IsAvailable = false };
        var addedBook = new Book { Title = "Added book", Author = "Added author", Year = 1234, Pages = 12, IsAvailable = false, Id = 1 };

        _mockRepository.Setup(s => s.AddAsync(inputBook)).ReturnsAsync(addedBook);

        // Act
        var result = await _service.AddAsync(inputBook);

        // Assert
        Assert.Equal(addedBook, result);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookExists_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result);
    }
}