using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace SAW.Web.Entities.Requests
{
    public class UserAddRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$",
            ErrorMessage = "Password must be between 8 and 20 characters and contain one uppercase letter, " +
            "one lowercase letter, one digit and one special character.")]
        public string Password { get; set; }

        public bool IsConfirmed { get; set; }

        public int Role { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
