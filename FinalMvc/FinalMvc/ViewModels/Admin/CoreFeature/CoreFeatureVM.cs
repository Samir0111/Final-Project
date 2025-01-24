using Microsoft.AspNetCore.Http;

namespace FinalMvc.ViewModels.Admin.CoreFeatures
{
    public class CoreFeatureVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
