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

        public MemberService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var emailexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var phoneexist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailexist || phoneexist) return false;

            //else true add ansber
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    Street = model.Street,
                    City = model.City,
                    BuildingNumber = model.BuildingNumber,
                },
                HealthRecord = new HealthRecord()
                {
                    Length = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    Note = model.HealthRecordViewModel.Note,
                    BloodType = model.HealthRecordViewModel.BloodType,
                }
            };

            unitOfWork.GetRepository<Member>().AddAsync(member);
            var res = await unitOfWork.SaveChangesAsync(ct);
            return res > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            var memberviewModel = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString(),
                Photo = m.Photo,
            });

            return memberviewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDetailsByIdAsync(int id, CancellationToken ct)
        {
            var query = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x=>x.MemberId == id);
            if (query == null)
                return null;
            else
                return new HealthRecordViewModel()
                {
                    Height = query.Length,
                    Weight = query.Weight,
                    BloodType = query.BloodType,
                    Note = query.Note,
                };
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return null;
            else return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
                Photo = member.Photo
            };

        }

        public async Task<MemberViewModel?> GetMemberDetailsByIdAsync(int id,CancellationToken ct)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id,ct);
            if(member == null) return null;

            var model = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $" {member.Address.BuildingNumber} {member.Address.Street} {member.Address.City}"
            };
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

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Phone = model.Phone;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
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
