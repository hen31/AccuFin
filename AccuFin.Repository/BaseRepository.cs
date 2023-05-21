using AccuFin.Data;

namespace AccuFin.Repository
{
    public class BaseRepository
    {
        protected AccuFinDatabaseContext DatabaseContext { get; set; }

        public BaseRepository(AccuFinDatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }
    }
}
