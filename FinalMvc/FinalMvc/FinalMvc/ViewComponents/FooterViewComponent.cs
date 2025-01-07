using Microsoft.AspNetCore.Mvc;
using FinalMvc.Services.Interfaces;

namespace FinalMvc.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;

        public FooterViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(await _layoutService.GetAllSettingsAsync()));
        }
    }
}
