using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using GymSystem.DAL.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BLL.ViewModels.TrainerViewModels
{

    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Name Is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$",
            ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Phone Number Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",
            ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
        public string Phone { get; set; } = default!;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public Specialty Specialty { get; set; }

        [Required]
        [Range(1, 9000)]
        public int BuildingNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; } = default!;

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Street { get; set; } = default!;
    }
}
