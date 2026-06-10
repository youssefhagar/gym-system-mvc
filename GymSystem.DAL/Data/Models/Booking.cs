using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class Booking : BaseEntity
    {

        public bool IsAttended { get; set; }

        #region Relations

        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }  

        public Session Session { get; set; } = default!;
        public int SessionId { get; set; }  


        #endregion

    }
}
