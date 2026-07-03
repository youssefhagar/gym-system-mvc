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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext ):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }


        public async Task<int> GetCountOfBookedSlots(int SessionId, CancellationToken ct = default)
        {
            return await dbContext.Bookings.AsNoTracking().CountAsync(x => x.SessionId == SessionId, ct);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategory(int id, CancellationToken ct = default)
        {
            return await dbContext.Sessions.AsNoTracking().Include(t => t.Trainer).Include(t => t.Category).FirstOrDefaultAsync(s=>s.Id==id,ct);
        }
    }
}
