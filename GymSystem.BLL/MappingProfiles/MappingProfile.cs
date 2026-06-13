using AutoMapper;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {

            #region Member

            CreateMap<Member, MemberViewModel>()
                .ForMember(des => des.Address, opt => opt.MapFrom(s => $"{s.Address.BuildingNumber} {s.Address.Street} {s.Address.City}"))
                .ForMember(des => des.DateOfBirth, opt => opt.MapFrom(s => s.DateOfBirth.ToShortDateString()));

            CreateMap<HealthRecord, HealthRecordViewModel>()
                .ForMember(des => des.Height, opt => opt.MapFrom(s => s.Length));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(s => s.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(s => s.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(des => des.Name, opt => opt.Ignore())
                .ForMember(des => des.Photo, opt => opt.Ignore())
                .AfterMap((src, des) =>
                {
                    des.Address.BuildingNumber = src.BuildingNumber;
                    des.Address.Street = src.Street;
                    des.Address.City = src.City;
                });

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(des => des.Address, opt => opt.MapFrom(s => new Address()
                {
                    BuildingNumber = s.BuildingNumber,
                    Street = s.Street,
                    City = s.City,
                }))
                .ForMember(des => des.HealthRecord, opt => opt.MapFrom(s => new HealthRecord()
                {
                    Length = s.HealthRecordViewModel.Height,
                    Weight = s.HealthRecordViewModel.Weight,
                    BloodType = s.HealthRecordViewModel.BloodType,
                    Note = s.HealthRecordViewModel.Note,
                }));


            #endregion


            #region Trainer

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(des => des.Specialty, opt => opt.MapFrom(s => s.Specialty.ToString()))
                .ForMember(des => des.Address, opt => opt.MapFrom(s => $"{s.Address.BuildingNumber} {s.Address.Street} {s.Address.City}"));

            CreateMap<Trainer, UpdateTrainerViewModel>()
                .ForMember(des => des.BuildingNumber, opt => opt.MapFrom(s => s.Address.BuildingNumber))
                .ForMember(des => des.Street, opt => opt.MapFrom(s => s.Address.Street))
                .ForMember(des => des.City, opt => opt.MapFrom(s => s.Address.City));

            CreateMap<UpdateTrainerViewModel, Trainer>()
            .ForMember(des => des.Name, opt => opt.Ignore())
            //.ForMember(des => des.Photo, opt => opt.Ignore())
            .AfterMap((src, des) =>
            {
                des.Address.BuildingNumber = src.BuildingNumber;
                des.Address.Street = src.Street;
                des.Address.City = src.City;
            });

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(des => des.Address, opt => opt.MapFrom(s => new Address()
                {
                    BuildingNumber = s.BuildingNumber,
                    Street = s.Street,
                    City = s.City,
                }));


            #endregion


            #region Plan

            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>();

            CreateMap<CreatePlanViewModel, Plan>()
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => true));

            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.Ignore())
                .ForMember(dest => dest.MemberShips,
                    opt => opt.Ignore());

            #endregion


        }

    }
}
