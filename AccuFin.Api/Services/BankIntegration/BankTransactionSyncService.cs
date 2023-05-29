using AccuFin.Data;
using AccuFin.Repository;
using System.Globalization;

namespace AccuFin.Api.Services.BankIntegration
{
    public class BankTransactionSyncService : IHostedService, IDisposable
    {
        private TimerAsync? _timer = null;
        private readonly IServiceProvider _services;

        public BankTransactionSyncService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new TimerAsync(DoWork, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }


        private async Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                var nordigenClient = scope.ServiceProvider.GetService<NordigenClient>();
                var dbContext = scope.ServiceProvider.GetService<AccuFinDatabaseContext>();
                var transactionRepository = scope.ServiceProvider.GetService<TransactionRepository>();
                foreach (var linkedAccount in dbContext.LinkBankAccounts.Where(b => b.Sync))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    var transactionsFromNordigen = await nordigenClient.GetTransactionsAsync(linkedAccount.AccountId);
                    foreach (var transaction in transactionsFromNordigen.Transactions.Booked)
                    {
                        DateTime transactionOn = DateTime.ParseExact(transaction.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        await transactionRepository.AddTransactionAsync(transaction.TransactionId,
                            linkedAccount.AdministrationId, transaction.DebtorName, transaction.DebtorAccount?.Iban,
                            transactionOn, transaction.RemittanceInformationUnstructured,
                            transaction.TransactionAmount.Amount, transaction.TransactionAmount.Currency, linkedAccount.IBAN);
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _timer?.StopAsync();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
