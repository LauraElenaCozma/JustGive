using JustGive.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(JustGive.Startup))]
namespace JustGive
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateNewRoles();
            

        }


        private void CreateNewRoles()
        {
            var ctx = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(ctx));
            // adaugam rolurile pe care le poate avea un utilizator
            // din cadrul aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // adaugam rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = userManager.Create(user, "AdminJustGive!");
                if (adminCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            /*if (!roleManager.RoleExists("Donator"))
            {
                // adaugati rolul specific aplicatiei voastre
                var role = new IdentityRole();
                role.Name = "Donator";
                roleManager.Create(role);
                // se adauga utilizatorul
                var user = new ApplicationUser();
                user.UserName = "cozmalaura23@gmail.com";
                user.Email = "cozmalaura23@gmail.com";
                var donatorCreated = userManager.Create(user, "DonatorJG2021!");
                if (donatorCreated.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Donator");
                }
            }*/
        }
    }
}
