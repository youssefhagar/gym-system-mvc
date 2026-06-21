using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface ISessionService
    {
        //index =>GetAllSession(ct) => SessionViewModel
        Task<IEnumerable<SessionViewModel>?> GetSessions(CancellationToken ct = default);
        Task<Result<SessionViewModel>?> GetSessionByIdAsync(int id, CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>?> GetCategoryForropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>?> GetTrainerForropDownAsync(CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>?> GetUpdateSessionsAsync(int id,CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);


    }
}
