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


    }
}
