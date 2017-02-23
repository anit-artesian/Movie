
using System.ComponentModel.DataAnnotations;

namespace MediaApplication.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
