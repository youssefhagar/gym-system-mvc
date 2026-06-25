using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.MembershipViewModels
{
    public class CreateMembershipViewModel
    {
        [Required(ErrorMessage = "Member  is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Member.")]
        public int MemberId { get; set; }


        [Required(ErrorMessage = "Plan is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid plan.")]
        public int PlanId { get; set; }


    }
}
