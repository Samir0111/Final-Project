namespace FinalMvc.Models
{
    public class AboutSection
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Pfp { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }

        public bool IsMain { get; set; }
    }
}
