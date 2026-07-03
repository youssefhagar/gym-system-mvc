using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, bool tracking = false, CancellationToken ct = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(
    Expression<Func<TEntity, bool>> predicate,
    bool tracking = false,
    CancellationToken ct = default);
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> perdicat, CancellationToken ct = default);
        
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> perdicat, bool tracking = false ,CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> perdicat = null!, CancellationToken ct = default); 

    }
}
