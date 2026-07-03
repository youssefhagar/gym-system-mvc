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


        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
           
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> perdicat, CancellationToken ct)
        {
            return _dbSet.AsNoTracking().AnyAsync(perdicat, ct);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> perdicat, CancellationToken ct = default)
        => perdicat == null ? await _dbSet.AsNoTracking().CountAsync(ct) : await _dbSet.AsNoTracking().CountAsync(perdicat, ct);
        public void DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> perdicat, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> Query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await Query.FirstOrDefaultAsync(perdicat,ct);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            if (predicate is not null) query = query.Where(predicate);
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)=> await _dbSet.FindAsync(id,ct);

        public async Task<TEntity?> GetByIdAsync(
    Expression<Func<TEntity, bool>> predicate,
    bool tracking = false,
    CancellationToken ct = default)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(predicate, ct);
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
           
        }
    }
}
