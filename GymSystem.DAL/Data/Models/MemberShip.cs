using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class MemberShip : BaseEntity
    {

        public DateTime EndDate { get; set; }
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
        public bool IsActive => EndDate > DateTime.Now;

        #region Relations

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        public Plan Plan { get; set; } = default!;
        public int PlanId { get; set; }


        #endregion

    }
}
