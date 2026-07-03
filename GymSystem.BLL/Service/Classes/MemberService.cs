using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Data.Models.Enums;
using GymSystem.DAL.Repository.Interfaces;


namespace GymSystem.BLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IAttachmentService attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IAttachmentService attachmentService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.attachmentService = attachmentService;
        }

        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailExist)
                return Result.Conflict("Email already exists");
            if (phoneExist)
                return Result.Conflict("Phone number already exists");

            //else true add ansber
            var member = mapper.Map<CreateMemberViewModel,Member>(model);

            var photo = await attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName,"MemberPicture" ,ct)!;
            if(string.IsNullOrEmpty(photo))
                return Result.Fail("Failed to upload photo");
            member.Photo = photo;

            await unitOfWork.GetRepository<Member>().AddAsync(member, ct);
            var result = await unitOfWork.SaveChangesAsync(ct);

            return result > 0 ? Result.Ok(): Result.Fail("Failed to create member");
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
                model.PlanName = plan!.Name;
                model.MembershipStartDate = activemembership.CreatedAt.ToString();
                model.MembershipEndDate = activemembership.EndDate.ToString();
            }

            return model;
        }

        public async Task<Result> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return Result.NotFound("Member not found");
            var emailExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id);

            if (emailExist)
                return Result.Conflict("Email already exists");
            if (phoneExist)
                return Result.Conflict("Phone number already exists");

            mapper.Map(model, member);
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to update member");
        }

        public async Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null)
                return Result.NotFound("Member not found");

            var hasfuturebooking = await unitOfWork.GetRepository<Booking>().AnyAsync(x=>x.MemberId==id && x.Session.StartDate > DateTime.Now);
            if(hasfuturebooking)
                return Result.Conflict("Cannot delete member because they have future bookings");

            unitOfWork.GetRepository<Member>().DeleteAsync(member);
            if(member.Photo is not null)
            {
               attachmentService.Delete(member.Photo, "MemberPicture");
                
            }
            var result = await unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to update member");

        }
    }
}
