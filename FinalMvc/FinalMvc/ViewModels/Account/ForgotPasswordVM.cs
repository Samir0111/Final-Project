using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
