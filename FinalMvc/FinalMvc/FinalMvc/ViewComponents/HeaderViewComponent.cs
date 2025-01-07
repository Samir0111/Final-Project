using Microsoft.AspNetCore.Mvc;
using FinalMvc.Services.Interfaces;
using System.Threading.Tasks;

namespace FinalMvc.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ILayoutService _layoutService;

        public HeaderViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = ViewData.Model;
            var settings = await _layoutService.GetAllSettingsAsync();


            ViewData["SettingsDictionary"] = settings;

            return View("Default", model);
        }
    }
}
