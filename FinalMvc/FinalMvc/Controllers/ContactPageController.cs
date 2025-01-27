using FinalMvc.Data;
using FinalMvc.Models;
using FinalMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMvc.Controllers
{
    public class ContactPageController : Controller
    {
        private readonly AppDbContext _context;

        public ContactPageController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var contact = await _context.Contacts.ToListAsync();

            return View(new ContactPageVM
            {

                Contacts = contact





            });


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitContact(Contact contact)
        {
           

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Index");
        }


    }
}
