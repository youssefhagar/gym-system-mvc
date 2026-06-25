using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IMembershipService
    {

        Task<IEnumerable<MembershipViewModel>?> GetAllMembershipsAsync(CancellationToken ct = default!);
        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default!);
        Task<IEnumerable<MemberSelectViewModel>?> GetMemberForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectViewModel>?> GetPlanForDropDownAsync(CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);


    }
}
