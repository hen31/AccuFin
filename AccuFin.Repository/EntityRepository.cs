﻿using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<FinCollection<ModelType>> GetCollectionAsync<ModelType>(int page, int pageSize, Expression<Func<T, bool>> where, Func<T, ModelType> convert)
        {
            var collection = new FinCollection<ModelType>();
            var fullSet =  DbSet.AsQueryable()
                .Where(where);
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
