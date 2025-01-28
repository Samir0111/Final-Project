using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Admin.AppointmentSection
{
    public class AppointmentSectionVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Catch is required.")]
        public string Catch { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subtitle is required.")]
        public string Subtitle { get; set; }

        public bool IsMain { get; set; }
    }
}
