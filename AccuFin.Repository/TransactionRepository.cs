using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using AccuFin.Data.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;
namespace AccuFin.Repository
{
    public class TransactionRepository : BaseRepository
    {
        public TransactionRepository(AccuFinDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Data.Entities.Transaction> AddTransactionAsync(string externalId,
            Guid administrationId, string debtorName, string debtorIBAN,
            DateTime transactionOn,
            string remittanceInformationUnstructured,
            string amount, string currency,
            string fromIban)
        {
            if (await DatabaseContext.Transactions.AnyAsync(b => 
            b.ExternalId == externalId //TODO: not always there
            && administrationId == b.AdministrationId))
            {
                return null;
            }

            var transaction = new Data.Entities.Transaction();
            transaction.ExternalId = externalId;
            transaction.AdministrationId = administrationId;
            transaction.FromIBAN = fromIban;
            transaction.TransactionDate = transactionOn;
            transaction.UnstructeredInformation = remittanceInformationUnstructured;
            transaction.Debtor = debtorName;
            transaction.ToIBAN = debtorIBAN;
            transaction.Amount = decimal.Parse(amount, System.Globalization.NumberStyles.Currency, new CultureInfo("en-US"));
            transaction.Currency = currency;
            var transactionRepository = new EntityRepository<Data.Entities.Transaction, ulong>(DatabaseContext);
            await transactionRepository.Add(transaction);
            return transaction;
        }

        public async Task<FinCollection<TransactionCollectionItem>> GetCollectionAsync(Guid administrationId, int page, int pageSize, string[] orderBy, string singleSearch)
        {
            EntityRepository<Data.Entities.Transaction, ulong> transactionRepository = new EntityRepository<Data.Entities.Transaction, ulong>(DatabaseContext);
            return await transactionRepository.GetCollectionAsync(page, pageSize, orderBy,
                b => b.AdministrationId == administrationId,
                b => b.MapForCollection());
        }
    }
}
