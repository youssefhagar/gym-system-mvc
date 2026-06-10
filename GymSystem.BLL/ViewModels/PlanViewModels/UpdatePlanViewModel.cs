using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name Is Required")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = default!;

        [Required]
        [Range(1, 3650)]
        public int DurationDays { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal Price { get; set; }
    }
}
