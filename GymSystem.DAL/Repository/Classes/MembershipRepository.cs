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
    public class MembershipRepository : IMembershipRepository
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<MemberShip> _dbSet;
        //public IGenericRepository<Member> Member { get; }
        //public IGenericRepository<Plan> Plan { get; }
        public MembershipRepository(GymDbContext dbContext/*,IGenericRepository<Member> membergenericRepository, IGenericRepository<Plan> plangenericRepository*/)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<MemberShip>();
            //Member = membergenericRepository;
            //Plan = plangenericRepository;
        }

        public async Task<int> AddAsync(MemberShip memberShip, CancellationToken ct = default)
        {
            _dbContext.MemberShips.Add(memberShip);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteAsync(MemberShip memberShip, CancellationToken ct = default)
        {
            _dbContext.MemberShips.Remove(memberShip);
            return await _dbContext.SaveChangesAsync(ct);
        }


        public async Task<MemberShip?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbContext.MemberShips.FindAsync(id,ct);
        }

        public async Task<IEnumerable<MemberShip>?> GetAllAsync(Expression<Func<MemberShip, bool>> perdicat = null!, bool tracking = false, CancellationToken ct = default)
        {
            try
            {
                if (perdicat == null)
                {
                    IQueryable<MemberShip> Query = tracking ? _dbSet.Include(X => X.Member).Include(X => X.Plan)
                        : _dbSet.AsNoTracking().Include(X => X.Member).Include(X => X.Plan);
                    return await Query.ToListAsync(ct);
                }
                else
                {
                    IQueryable<MemberShip> Query = tracking ? _dbSet.Where(perdicat).Include(X => X.Member).Include(X => X.Plan)
                        : _dbSet.AsNoTracking().Where(perdicat).Include(X => X.Member).Include(X => X.Plan);
                    return await Query.ToListAsync(ct);
                }
            
            }
            catch (Exception)
            {

                return null;
            }

           

        }

        public async Task<Member?> GetMemberByIdAsync(Expression<Func<Member, bool>> perdicat , bool tracking = false, CancellationToken ct = default)
        {
            try
            {
                var query = tracking ? await _dbContext.Set<Member>().Where(perdicat).Include(X => X.MemberShips).FirstOrDefaultAsync(ct)
                    : await _dbContext.Set<Member>().AsNoTracking().Where(perdicat).Include(X => X.MemberShips).FirstOrDefaultAsync(ct);
               
                return query;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Plan?> GetPlanByIdAsync(Expression<Func<Plan, bool>> perdicat, bool tracking = false, CancellationToken ct = default)
        {
            try
            {
                var query = tracking ? await _dbContext.Set<Plan>().Where(perdicat).Include(X => X.MemberShips).FirstOrDefaultAsync(ct)
                    : await _dbContext.Set<Plan>().AsNoTracking().Where(perdicat).Include(X => X.MemberShips).FirstOrDefaultAsync(ct);

                return query;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
