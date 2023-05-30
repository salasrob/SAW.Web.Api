using System.ComponentModel.DataAnnotations;

namespace SAW.Web.Entities.Requests
{
    public class UserForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}
