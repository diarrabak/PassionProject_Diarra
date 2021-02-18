using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PassionProjectMVP_Diarra.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class PassionDataContext : IdentityDbContext<ApplicationUser>
    {
        public PassionDataContext()
            : base("PassionDataContextMVP", throwIfV1Schema: false)
        {
        }

        public static PassionDataContext Create()
        {
            return new PassionDataContext();
        }
    

        //Instruction to set the models as tables in our database.

        public DbSet<Classe> Classes { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Location> Locations { get; set; }
    }



}