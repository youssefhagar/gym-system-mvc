using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticsViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync();
            var totalmembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:ct);
            var totaltrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct);
            var activemembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync(m => m.EndDate > DateTime.Now,ct);


            return new AnalyticsViewModel
            {
                TotalMembers = totalmembers,
                ActiveMembers = activemembers,
                TotalTrainers = totaltrainers,
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.Now)
            };

        }
    }
}
