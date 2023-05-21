using AccuFin.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuFin.Api.Client
{
    public interface ICollectionSourceClient<T>
    {

        Task<Response<FinCollection<T>, List<ValidationError>>> GetCollectionAsync(int page, int pageSize, string[] orderBy, string singleSearchText);

    }
}