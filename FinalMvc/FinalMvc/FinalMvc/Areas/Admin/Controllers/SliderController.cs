using FinalMvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FinalMvc.Data;
using FinalMvc.Models;
using Microsoft.EntityFrameworkCore;
using FinalMvc.ViewModels.Admin.Slider;
using Microsoft.AspNetCore.Authorization;


namespace FinalMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class SliderController : Controller
    {



    }
}
