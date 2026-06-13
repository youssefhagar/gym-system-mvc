using AutoMapper;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }




        public async Task<IEnumerable<SessionViewModel>?> GetSessions(CancellationToken ct = default)
        {
            var sessions = await unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory(ct);

            if (sessions == null || !sessions.Any())
                return null;

            var sessionsViewModel = mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in sessionsViewModel)
            {
                session.AvailableSlots = session.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id, ct);
            }

            return sessionsViewModel;

        }
    }
}
