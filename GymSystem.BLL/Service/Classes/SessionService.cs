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

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate || model.StartDate <= DateTime.Now) return false;
            if(model.Capacity <1 ||  model.Capacity > 25) return false;

            var trainerexist = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            var categoryexist = await unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);

            if(trainerexist == null || categoryexist == null) return false;

            var session = mapper.Map<CreateSessionViewModel,Session>(model);
            unitOfWork.GetRepository<Session>().AddAsync(session);

            return await unitOfWork.SaveChangesAsync(ct) > 0;

        }

        public async Task<IEnumerable<CategorySelectViewModel>?> GetCategoryForropDownAsync(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
            if (result == null || !result.Any())
                return null;

            return mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
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

        public async Task<IEnumerable<TrainerSelectViewModel>?> GetTrainerForropDownAsync(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (result == null || !result.Any())
                return null;

            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(result);
        }
    }
}
