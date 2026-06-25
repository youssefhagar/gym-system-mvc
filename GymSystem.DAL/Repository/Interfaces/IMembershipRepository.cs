using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<MemberShip>> GetAllAsync(Expression<Func<MemberShip, bool>> perdicat=null! ,bool tracking = false, CancellationToken ct = default);
        Task<MemberShip?> GetByIdAsync(int id, CancellationToken ct = default);
      
        Task<int> AddAsync(MemberShip memberShip, CancellationToken ct = default);
        public Task<int> DeleteAsync(MemberShip memberShip, CancellationToken ct = default);
        Task<Plan?> GetPlanByIdAsync(Expression<Func<Plan, bool>> perdicat ,bool tracking = false, CancellationToken ct = default);
        Task<Member?> GetMemberByIdAsync(Expression<Func<Member, bool>> perdicat ,bool tracking = false, CancellationToken ct = default);
        //Task<Member?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> perdicat, bool tracking = false, CancellationToken ct = default);
    }
}
