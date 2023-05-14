using AccuFin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
