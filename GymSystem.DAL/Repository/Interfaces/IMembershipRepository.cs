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
    public interface IMembershipRepository : IGenericRepository<MemberShip>
    {
        Task<IEnumerable<MemberShip>> GetAllAsync(Expression<Func<MemberShip, bool>> perdicat=null! ,bool tracking = false, CancellationToken ct = default);
        Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null, CancellationToken ct = default);
        Task<Member?> GetMemberByIdAsync(Expression<Func<Member, bool>> predicate, CancellationToken ct = default);
        Task<Plan?> GetPlanByIdAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct = default);
    //    Task<Member?> GetMemberByIdAsync(
    //Expression<Func<Member, bool>> predicate,
    //CancellationToken ct = default);
    }
}
