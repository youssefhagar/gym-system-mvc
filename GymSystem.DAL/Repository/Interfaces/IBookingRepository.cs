
using GymSystem.DAL.Data.Models;

namespace GymSystem.DAL.Repository.Interfaces
{
	public interface IBookingRepository : IGenericRepository<Booking>
	{
        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default);

    }
}
