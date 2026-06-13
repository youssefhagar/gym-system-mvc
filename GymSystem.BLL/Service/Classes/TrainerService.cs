using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken ct = default) // Added outomapper
        {
            var trainers = await unitOfWork
                .GetRepository<Trainer>()
                .GetAllAsync(ct: ct);

            return mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerViewModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id, ct);

            if (trainer is null)
                return null;

            return mapper.Map<TrainerViewModel>(trainer);
        }

        public async Task<Result> CreateAsync( CreateTrainerViewModel model,CancellationToken ct = default)
        {
            var emailExists = await unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);

            if (emailExists)
                return Result.Conflict("Email already exists");

            var trainer = mapper.Map<Trainer>(model);

            unitOfWork.GetRepository<Trainer>().AddAsync(trainer);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0
                ? Result.Ok()
                : Result.Fail("Failed to create trainer");
        }

        public async Task<Result> UpdateAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id, ct);

            if (trainer is null)
                return Result.NotFound("Trainer not found");

            mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);
            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update trainer");
        }

        public async Task<Result> DeleteAsync( int id,CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id, ct);

            if (trainer is null)
                return Result.NotFound("Trainer not found");

            unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok(): Result.Fail("Failed to delete trainer");
        }

        public async Task<UpdateTrainerViewModel?> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return null;

            return mapper.Map<UpdateTrainerViewModel>(trainer);
        }
    }
}
