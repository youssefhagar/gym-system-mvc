using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults; // Add this if not already present
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class MembershipService : IMembershipService
    {


        private readonly IUnitOfWork _unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct)
        {
            var member = await _unitOfWork.MembershipRepository.GetMemberByIdAsync(x=>x.Id == model.MemberId, ct: ct);
            var plan = await _unitOfWork.MembershipRepository.GetPlanByIdAsync(x => x.Id == model.PlanId, ct: ct);

            if (member == null)
                return Result.NotFound($"Please Select vaild member .");

            if(plan == null)
                return Result.NotFound($"Please Select vaild plan .");
            
            //if(member.MemberShips.EndDate)
            //var membership = await _unitOfWork.MembershipRepository.GetAllAsync(X=>X.MemberId == model.MemberId, ct: ct);
            foreach ( var item in member.MemberShips)
            {
                if (item == null) continue;
                if (item.IsActive)
                    return null!;
            }
            foreach ( var item in plan.MemberShips)
            {
                if (item == null) continue;
                if (!item.IsActive)
                    return null!;
            }
            MemberShip mappedMemberShip = new MemberShip
            {

                MemberId = model.MemberId,
                PlanId = model.PlanId,
                EndDate = DateTime.Now.AddDays(plan.DurationDays),
            };

            var result = await _unitOfWork.MembershipRepository.AddAsync(mappedMemberShip, ct: ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create membership.", ResultKind.Conflict);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            
            var member = await _unitOfWork.MembershipRepository.GetMemberByIdAsync(x => x.Id == id, ct: ct); ;

            if (member == null)
                return Result.NotFound($"Member not found.");

            foreach (var item in member.MemberShips)
            {
                if (item == null) continue;
                if (item.IsActive)
                {
                    var result = await _unitOfWork.MembershipRepository.DeleteAsync(item, ct);
                    return result > 0 ? Result.Ok() : Result.Fail("Failed to delete membership.", ResultKind.Conflict);
                }
            }

            return Result.NotFound($"Active membership not found for the member.");

        }

        public async Task<IEnumerable<MembershipViewModel>?> GetAllMembershipsAsync(CancellationToken ct = default!)
        {
            try
            {
                var memberships = await _unitOfWork.MembershipRepository.GetAllAsync(ct: ct);
                if (memberships == null || !memberships.Any())
                {
                    return Enumerable.Empty<MembershipViewModel>();
                }
                return memberships.Select(m => new MembershipViewModel
                {
                    MemberId = m.MemberId,
                    MemberName = m.Member.Name,
                    PlanName = m.Plan.Name,
                    StartDate = m.CreatedAt,
                    EndDate = m.EndDate
                });
            }
            catch (Exception)
            {

                return Enumerable.Empty<MembershipViewModel>();
            }
            

        }

        public async Task<IEnumerable<MemberSelectViewModel>?> GetMemberForDropDownAsync(CancellationToken ct = default)
        {
            try
            {
                var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
                if (members == null || !members.Any())
                {
                    return Enumerable.Empty<MemberSelectViewModel>();
                }
                return members.Select(m => new MemberSelectViewModel
                {
                    MemberId = m.Id,
                    MemberName = m.Name
                });
            }
            catch (Exception)
            {

                return Enumerable.Empty<MemberSelectViewModel>();
            }
        }

        public async Task<IEnumerable<PlanSelectViewModel>?> GetPlanForDropDownAsync(CancellationToken ct = default)
        {
            try
            {
                var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
                if (plans == null || !plans.Any())
                {
                    return Enumerable.Empty<PlanSelectViewModel>();
                }
                return plans.Select(p => new PlanSelectViewModel
                {
                    PlanId = p.Id,
                    PlanName = p.Name
                });
            }
            catch (Exception)
            {

                return Enumerable.Empty<PlanSelectViewModel>();
            }
        }
    }
}
