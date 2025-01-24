using FinalMvc.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalMvc.ViewModels.Account
{
    public class AdjustRolesVM
    {
        public string UserId { get; set; }

        public List<AppUser> Users { get; set; } = new List<AppUser>();

        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();

        public string SelectedRole { get; set; }
    }
}
