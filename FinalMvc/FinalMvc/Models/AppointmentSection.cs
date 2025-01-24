namespace FinalMvc.Models
{
    public class AppointmentSection
    {
        public int Id { get; set; }
        public string Catch { get; set; } 
        public string Title { get; set; } 
        public string Subtitle { get; set; }

        public bool IsMain { get; set; }
    }
}
