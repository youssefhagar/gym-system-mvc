using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default);
        Task<Result> CreateMemberAsync(CreateMemberViewModel model,CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default);


    }
}
