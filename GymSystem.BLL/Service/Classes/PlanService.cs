using AutoMapper;
using GymSystem.BLL.Common;
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
        private readonly IMapper mapper;

        public PlanService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork
                .GetRepository<Plan>()
                .GetAllAsync(ct: ct);

            return mapper.Map<IEnumerable<PlanViewModel>>(plans); ;
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return null;

            return mapper.Map<PlanViewModel>(plan);
        }

        public async Task<Result> CreatePlanAsync(CreatePlanViewModel model, CancellationToken ct = default)
        {
            var exists = await unitOfWork
                .GetRepository<Plan>()
                .AnyAsync(x => x.Name == model.Name, ct);

            if (exists)
                return Result.Conflict("Plan name already exists");

            var plan = mapper.Map<Plan>(model);

            unitOfWork.GetRepository<Plan>().AddAsync(plan);
            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok(): Result.Fail("Failed to create plan");
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model,CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return Result.NotFound("Plan not found");

            mapper.Map(model, plan);
            plan.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Plan>().UpdateAsync(plan);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan");
        }

        public async Task<Result> DeletePlanAsync(int id,
            CancellationToken ct = default)
        {
            var plan = await unitOfWork
                .GetRepository<Plan>()
                .GetByIdAsync(id, ct);

            if (plan is null)
                return Result.NotFound("Plan not found");

            plan.IsActive = false;

            unitOfWork.GetRepository<Plan>().UpdateAsync(plan);

            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Remove plan");
        }
    }
}
