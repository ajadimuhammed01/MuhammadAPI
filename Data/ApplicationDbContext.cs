using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using techHowdy.API.Models;

namespace techHowdy.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           
        }
        //Creating roles for applications
        public DbSet<ProductModel> Products {get; set;}
        public DbSet<ApplicationUser> ApplicationUsers {get; set;}
    }
}