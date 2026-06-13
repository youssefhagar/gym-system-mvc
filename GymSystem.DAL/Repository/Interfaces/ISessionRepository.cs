using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface ISessionRepository :IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategory(CancellationToken ct = default);
        Task<Session> GetSessionWithTrainerAndCategory(int id, CancellationToken ct = default);
        Task<int> GetCountOfBookedSlots(int SessionId,CancellationToken ct = default);

    }
}
