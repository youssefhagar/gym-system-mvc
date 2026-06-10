using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    [Owned]
    public class Address
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public int BuildingNumber { get; set; }
    }
}
