using LibraryCatalog.Services;

namespace LibraryCatalog.BackgroundServices;

public class ActiveLoansReportService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<ActiveLoansReportService> _logger;

    public ActiveLoansReportService(IServiceScopeFactory factory, ILogger<ActiveLoansReportService> logger)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            using (var scope = _factory.CreateScope())
            {
                var loanService = scope.ServiceProvider.GetRequiredService<ILoanService>();
                var allLoans = await loanService.GetAllAsync();

                var activeLoans = allLoans.Count(l => l.ReturnDate == null);
                _logger.LogInformation("There are {Count} active loans", activeLoans);
            }
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}