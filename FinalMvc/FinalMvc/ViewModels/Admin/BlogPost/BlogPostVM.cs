using Microsoft.AspNetCore.Http;

namespace FinalMvc.ViewModels.Admin.BlogPosts
{
    public class BlogPostVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public string ByWho { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
