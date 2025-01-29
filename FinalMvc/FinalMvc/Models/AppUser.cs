using Microsoft.AspNetCore.Identity;

namespace FinalMvc.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        
        public virtual ICollection<Reservation> Reservations
        {
            get; set;

        }
    }
}