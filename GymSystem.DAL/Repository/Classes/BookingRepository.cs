
using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repository.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
	{
		private readonly GymDbContext _dbContext;

		public BookingRepository(GymDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default)
            => _dbContext.Bookings.AsNoTracking().Include(b => b.Member).Where(b => b.SessionId == sessionId).ToListAsync(ct);

      
    }
}
