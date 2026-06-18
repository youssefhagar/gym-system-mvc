using AutoMapper;
using GymSystem.BLL.Common;
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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future");

            if (model.Capacity < 1 || model.Capacity > 25)
                return Result.Validation("Capacity must be between 1 and 25");

            var trainerExist = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            var categoryExist = await unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);

            if (trainerExist is null)
                return Result.NotFound("Trainer not found");

            if (categoryExist is null)
                return Result.NotFound("Category not found");

            var session = mapper.Map<CreateSessionViewModel,Session>(model);
            unitOfWork.GetRepository<Session>().AddAsync(session);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to create session");

        }

        public async Task<IEnumerable<CategorySelectViewModel>?> GetCategoryForropDownAsync(CancellationToken ct = default)
        {
            var result = await unitOfWork.GetRepository<Category>().GetAllAsync(ct:ct);
            if (result == null || !result.Any())
                return null;

            return mapper.Map<IEnumerable<CategorySelectViewModel>>(result);
        }

        public async Task<Result<SessionViewModel>?> GetSessionByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(id,ct);
            if (session == null)
                return Result<SessionViewModel>.NotFound("SessionNot Found");
            else
            {
                var mappedSession =  mapper.Map<SessionViewModel>(session);
                mappedSession.AvailableSlots = mappedSession.Capacity - await unitOfWork.SessionRepository.GetCountOfBookedSlots(id, ct);
                return Result<SessionViewModel>.Ok(mappedSession);
            }
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
