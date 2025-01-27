using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels
{
    public class ContactVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters.")]
        public string Message { get; set; }

        public string SuccessMessage { get; set; }
    }
}
