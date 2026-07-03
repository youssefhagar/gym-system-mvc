using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GymSystem.DAL.Repository.Classes
{
    public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
    {
        private readonly GymDbContext _dbContext;

        public MembershipRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null,
           CancellationToken ct = default)
        {
            IQueryable<MemberShip> query = _dbContext.MemberShips.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

        public async Task<Member?> GetMemberByIdAsync(Expression<Func<Member, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Members
                .AsNoTracking()
                .Include(m => m.MemberShips)
                .FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<Plan?> GetPlanByIdAsync(Expression<Func<Plan, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Plans
                .AsNoTracking()
                .Include(p => p.MemberShips)
                .FirstOrDefaultAsync(predicate, ct);
        }

    }
}
