namespace LibraryCatalog.Services;

public interface IAuthService
{
    public string? Login(string username, string password);
}