using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MembershipViewModels
{
    public class MemberSelectViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;
    }
}
