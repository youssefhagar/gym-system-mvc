using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IPlanRepository
    {

        public  Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        public  Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> AddAsync(Plan plan, CancellationToken ct = default);
        public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default);
        public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default);


    }
}
