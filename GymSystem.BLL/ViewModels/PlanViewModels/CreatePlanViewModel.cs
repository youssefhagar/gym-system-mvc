using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.PlanViewModels
{
    public class CreatePlanViewModel
    {
        [Required(ErrorMessage = "Plan Name Is Required")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(500, MinimumLength = 10,
            ErrorMessage = "Description must be between 10 and 500 characters")]
        public string Description { get; set; } = default!;

        [Required]
        [Range(1, 3650,
            ErrorMessage = "Duration must be between 1 and 3650 days")]
        public int DurationDays { get; set; }

        [Required]
        [Range(1, 100000,
            ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
