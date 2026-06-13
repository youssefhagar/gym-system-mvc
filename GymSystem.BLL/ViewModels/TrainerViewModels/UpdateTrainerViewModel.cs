using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.DAL.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{
   
    public class UpdateTrainerViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [RegularExpression(@"^(010|011|012|015)\d{8}$")]
        public string Phone { get; set; } = default!;

        [Required]
        public Specialty? Specialty { get; set; }

        [Required]
        [Range(1, 9000)]
        public int BuildingNumber { get; set; }

        [Required]
        public string City { get; set; } = default!;

        [Required]
        public string Street { get; set; } = default!;
    }
}
