using AutoMapper;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Data.Models.Enums;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var emailexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var phoneexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailexist || phoneexist) return false;

            //else true add ansber
            var member = mapper.Map<CreateMemberViewModel,Member>(model);

            unitOfWork.GetRepository<Member>().AddAsync(member);
            var res = await unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            
            var memberviewModel = mapper.Map<IEnumerable<MemberViewModel>>(members);

            return memberviewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDetailsByIdAsync(int id, CancellationToken ct)
        {
            var query = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x=>x.MemberId == id);
            if (query == null)
                return null;
            else
                return mapper.Map<HealthRecordViewModel>(query);
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return null;
            else return mapper.Map<MemberToUpdateViewModel>(member);

        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int id,CancellationToken ct)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id,ct);
            if(member == null) return null;

            var model = mapper.Map<MemberViewModel>(member);
            var activemembership = await unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x=>x.MemberId == id && x.EndDate > DateTime.Now);
            if (activemembership is not null)
            {
                var plan = await unitOfWork.GetRepository<Plan>().GetByIdAsync(activemembership.PlanId);
                model.PlanName = plan.Name;
                model.MembershipStartDate = activemembership.CreatedAt.ToString();
                model.MembershipEndDate = activemembership.EndDate.ToString();
            }

            return model;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return false;
            var emailexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id);
            var phoneexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id);

            if (emailexist || phoneexist) return false;

            mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var res = await unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)return false;

            var hasfuturebooking = await unitOfWork.GetRepository<Booking>().AnyAsync(x=>x.MemberId==id && x.Session.StartDate > DateTime.Now);
            if(hasfuturebooking) return false;

            unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var res = await unitOfWork.SaveChangesAsync(ct);
            return res > 0;

        }
    }
}
