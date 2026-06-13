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

        public TrainerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var trainers = await unitOfWork
                .GetRepository<Trainer>()
                .GetAllAsync(ct: ct);

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialty = t.Specialty.ToString()
            });
        }

        public async Task<TrainerViewModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var trainer = await unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id, ct);

            if (trainer is null)
                return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialty.ToString(),
                //Gender = trainer.Gender.ToString(),
                //DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                //Address = trainer.Address.Street

            };
        }

        public async Task<bool> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await unitOfWork
                .GetRepository<Trainer>()
                .AnyAsync(x => x.Email == model.Email, ct);

            if (emailExists)
                return false;

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Specialty = model.Specialty,
                Address = new Address
                {
                    Street = model.Street,
                    City = model.City,
                    BuildingNumber = model.BuildingNumber
                }
            };

            unitOfWork.GetRepository<Trainer>().AddAsync(trainer);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdateAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer is null)
                return false;

            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialty = model.Specialty.Value;

            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.Address.BuildingNumber = model.BuildingNumber;

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

            return new UpdateTrainerViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialty = trainer.Specialty,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street

            };
        }
    }
}
