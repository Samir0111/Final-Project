using Microsoft.AspNetCore.Http;

namespace FinalMvc.ViewModels.Admin.VideoPreviews
{
    public class VideoPreviewVM
    {
        public int Id { get; set; }
        public string YtLink { get; set; }
        public string PreviewImage { get; set; }
        public IFormFile Photo { get; set; }
        public bool IsMain { get; set; }
    }
}
