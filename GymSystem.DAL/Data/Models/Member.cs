using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo {  get; set; }

        //join date === CreatedAt

        #region Relations

        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MemberShips { get; set; } = default!;
        public ICollection<Booking> MemberSessions { get; set; } = default!;


        #endregion

    }
}
