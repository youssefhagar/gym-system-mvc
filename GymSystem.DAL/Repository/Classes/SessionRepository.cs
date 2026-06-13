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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext dbContext;

        public SessionRepository(GymDbContext dbContext ):base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategory(CancellationToken ct = default)
        {
            var query = dbContext.Sessions.AsNoTracking().Include(t=>t.Trainer).Include(t=>t.Category);
            return await query.ToListAsync();
        }

        public async Task<int> GetCountOfBookedSlots(int SessionId, CancellationToken ct = default)
        {
            return await dbContext.Bookings.AsNoTracking().CountAsync(x => x.SessionId == SessionId);
        }
    }
}
