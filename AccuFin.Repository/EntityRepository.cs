using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccuFin.Repository
{
    public class EntityRepository<T, IdType> : BaseRepository where T : BaseEntity<IdType>
    {
        protected DbSet<T> DbSet { get; set; }
        public EntityRepository(AccuFinDatabaseContext databaseContext) : base(databaseContext)
        {
            DbSet = databaseContext.Set<T>();
        }

        public async Task<T> GetById(IdType id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<FinCollection<ModelType>> GetCollectionAsync<ModelType>(int page, int pageSize, string[] orderBy, Expression<Func<T, bool>> where, Func<T, ModelType> convert)
        {
            var collection = new FinCollection<ModelType>();
            var fullSet = DbSet.AsQueryable()
                .Where(where);
            if (orderBy?.Length > 0)
            {
                for (int i = 0; i < orderBy.Length; i++)
                {

                    var parts = orderBy[i].Split(';');
                    if (parts.Length != 2)
                    {
                        continue;
                    }
                    if (!bool.TryParse(parts[1], out bool descending))
                    {
                        continue;
                    }
                    if (i == 0)
                    {
                        fullSet = fullSet.OrderBy(parts[0], descending);
                    }
                    else
                    {
                        fullSet = fullSet.ThenBy(parts[0], descending);
                    }
                }
            }
            else
            {
                fullSet = fullSet.OrderBy(b => b.Id);
            }
            var set = await
                fullSet.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => convert.Invoke(b))
                .ToListAsync();
            collection.Items = set;
            collection.Count = await fullSet.CountAsync();
            return collection;
        }

        public async Task Add(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

    }
}
