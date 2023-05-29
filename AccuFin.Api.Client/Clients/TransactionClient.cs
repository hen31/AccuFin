using AccuFin.Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public class TransactionClient : BaseClient
    {
        public TransactionClient(HttpClient httpClient,
            IClientAuthentication contextProvider) : base(httpClient, contextProvider)
        {
            Area = "transaction";
        }

        public Task<Response<FinCollection<TransactionCollectionItem>, List<ValidationError>>> GetCollectionAsync(Guid AdministrationId, int page, int pageSize, string[] orderBy, string singleSearchText)
        {
            return DoGetRequest<FinCollection<TransactionCollectionItem>, List<ValidationError>>($"/{AdministrationId}" + GenerateCollectionParametersUrl(page, pageSize, orderBy, singleSearchText));
        }

        public ICollectionSourceClient<TransactionCollectionItem> GetCollectionSource(Guid administrationId)
        {
            return new TransactionsForAdministrationSource(administrationId, this);
        }
    }

    public class TransactionsForAdministrationSource : ICollectionSourceClient<TransactionCollectionItem>
    {
        private Guid _adminstrationId;
        private readonly TransactionClient _transactionClient;

        public TransactionsForAdministrationSource(Guid adminstrationId, TransactionClient transactionClient)
        {
            _adminstrationId = adminstrationId;
            _transactionClient = transactionClient;
        }

        public Task<Response<FinCollection<TransactionCollectionItem>, List<ValidationError>>> GetCollectionAsync(int page, int pageSize, string[] orderBy, string singleSearchText)
        {
            return _transactionClient.GetCollectionAsync(_adminstrationId, page, pageSize, orderBy, singleSearchText);
        }
    }
}
