using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<String, Object> repositories = [];

        public UnitOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if(repositories.TryGetValue(typeName, out object? repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                repositories[typeName] = repo;
                return repo;
            }


        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _dbContext.SaveChangesAsync(ct);
    
    
    }
}