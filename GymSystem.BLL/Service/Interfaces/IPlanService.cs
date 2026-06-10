using GymSystem.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanByIdAsync(int id,
            CancellationToken ct = default);

        Task<bool> CreatePlanAsync(CreatePlanViewModel model,
            CancellationToken ct = default);

        Task<bool> UpdatePlanAsync(int id,
            UpdatePlanViewModel model,
            CancellationToken ct = default);

        Task<bool> DeletePlanAsync(int id,
            CancellationToken ct = default);
    }
}
