using E_Commerce.Domian.Entites;
using E_Commerce.Domian.Interfaces;
using E_Commerce.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity);

            var flag = _repositories.TryGetValue(key, out var repository);

            if (repository == null || flag == false)
            {
                repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }
            return (IGenericRepository<TEntity>)repository;
        }



    }
}
