using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }


        public void AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
           
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> perdicat, CancellationToken ct)
        {
            return _dbSet.AsNoTracking().AnyAsync(perdicat, ct);
        }

        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> perdicat, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> Query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await Query.FirstOrDefaultAsync(perdicat,ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> Query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await Query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FindAsync(id,ct);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
           
        }
    }
}
