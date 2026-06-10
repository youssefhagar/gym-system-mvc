using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public string? Note { get; set; }
        public string BloodType { get; set; } = default!;

        // LastUpdated == UpdatedAt



        #region Relations

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        #endregion

    }
}
