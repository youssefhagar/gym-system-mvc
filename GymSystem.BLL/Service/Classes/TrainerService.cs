using AutoMapper;
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

        public async Task<bool> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(x => x.Email == model.Email, ct);

            if (emailExists)
                return false;

            var trainer = mapper.Map<CreateTrainerViewModel, Trainer>(model);

            unitOfWork.GetRepository<Trainer>().AddAsync(trainer);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdateAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return false;

            mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteAsync( int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id, ct);

            if (trainer is null)
                return false;

            unitOfWork.GetRepository<Trainer>()
                .DeleteAsync(trainer);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
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
