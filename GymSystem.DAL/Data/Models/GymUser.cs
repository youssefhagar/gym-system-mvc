using GymSystem.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class GymUser : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public DateOnly DateOfBirth { get; set; }
        public Address Address { get; set; } = default!;
        public Gender Gender { get; set; }


    }

   


}
