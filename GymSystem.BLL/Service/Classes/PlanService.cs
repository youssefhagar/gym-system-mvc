using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork
                .GetRepository<Plan>()
                .GetAllAsync(ct: ct);

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public async Task<bool> CreatePlanAsync(CreatePlanViewModel model, CancellationToken ct = default)
        {
            var exists = await unitOfWork
                .GetRepository<Plan>()
                .AnyAsync(x => x.Name == model.Name, ct);

            if (exists)
                return false;

            var plan = new Plan
            {
                Name = model.Name,
                Description = model.Description,
                DurationDays = model.DurationDays,
                Price = model.Price,
                IsActive = true
            };

            unitOfWork.GetRepository<Plan>().AddAsync(plan);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id,
            UpdatePlanViewModel model,
            CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return false;

            plan.Name = model.Name;
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Plan>().UpdateAsync(plan);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeletePlanAsync(int id,
            CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return false;

            plan.IsActive = false;

            unitOfWork.GetRepository<Plan>().UpdateAsync(plan);

            return await unitOfWork.SaveChangesAsync(ct) > 0;
        }
    }
}
