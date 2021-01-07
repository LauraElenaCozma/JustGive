using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JustGive.Models
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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        }

        public DbSet<Donation> Donations { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Cause> Causes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Initp : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    { // custom initializer
        protected override void Seed(ApplicationDbContext ctx)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(ctx));
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(ctx));

            if (!roleManager.RoleExists("Donator"))
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
            }
            var userId = ctx.Users.SingleOrDefaultAsync(u => u.UserName.Equals("cozmalaura23@gmail.com")).Result.Id;

            Location location1 = new Location
            {
                City = "Bucuresti",
                Country = "Romania"
            };
            Location location2 = new Location
            {
                City = "Cluj-Napoca",
                Country = "Romania"
            };
            Location location3 = new Location
            {
                City = "Roman",
                Country = "Romania"
            };
            ctx.Locations.Add(location1);
            ctx.Locations.Add(location2);
            ctx.Locations.Add(location3);

            Tag tag1 = new Tag { Name = "copii" };
            Tag tag2 = new Tag { Name = "jucarii" };
            Tag tag3 = new Tag { Name = "carti" };
            Tag tag4 = new Tag { Name = "scoala" };
            Tag tag5 = new Tag { Name = "obiecte casnice" };

            ctx.Tags.Add(tag1);
            ctx.Tags.Add(tag2);
            ctx.Tags.Add(tag3);
            ctx.Tags.Add(tag4);
            ctx.Tags.Add(tag5);

            ContactInfo ctInfo1 = new ContactInfo { Name = "Stefania Iftodi", PhoneNumber = "0723941889", BirthDate = new System.DateTime(2010, 9, 13) };
            ContactInfo ctInfo2 = new ContactInfo { Name = "Mihai Andriciuc", PhoneNumber = "0749921308", BirthDate = new System.DateTime(2014, 12, 3)};
            //TODO: delete this or not?
            //ContactInfo ctInfo3 = new ContactInfo { Name = "Mirela Oniciuc", PhoneNumber = "0233940017" };
            ctx.ContactInfos.Add(ctInfo1);
            ctx.ContactInfos.Add(ctInfo2);
            //ctx.ContactInfos.Add(ctInfo3);

            
            Donation donation1 = new Donation
            {
                Title = "Jucarii de plus",
                Description = "Ofer 3 saci jucarii de plus potrivite pentru copii cu varsta intre 3 si 12 ani.",
                Location = location1,
                Tags = new List<Tag>
                {
                    tag1,
                    tag2
                },
                UserId = userId
            };
            Donation donation2 = new Donation
            {
                Title = "Carti",
                Description = "Donez 10 carti de povesti potrivite pentru copii.",
                Location = location3,
                Tags = new List<Tag>
                {
                    tag1,
                    tag3,
                    tag4

                },
                UserId = userId
            };
            Donation donation3 = new Donation
            {
                Title = "Carti",
                Description = "Donez 10 carti de povesti potrivite pentru copii.",
                Location = location2,
                Tags = new List<Tag>
                {
                    tag1,
                    tag3,
                    tag4

                },
                UserId = userId
            };
            ctx.Donations.Add(donation1);
            ctx.Donations.Add(donation2);
            ctx.Donations.Add(donation3);

            ctx.Causes.Add(new Cause
            {
                Title = "Salveaz-o pe Stefania",
                Description = "Stefania este o fetita de 10 ani din comuna Bulimaci, Jud. Ilfov" +
                                ", ai carei parinti nu au niciun venit. Jucarii, carti, totul este bine-venit pentru a-i face viata  mai frumoasa.",
                Location = location1,
                Tags = new List<Tag>
                {
                    tag1,
                    tag2,
                    tag3,
                    tag4
                },
                ContactInfo = ctInfo1,
                UserId = userId
            });
            ctx.Causes.Add(new Cause
            {
                Title = "Ajuta familia Andriciuc",
                Description = "De Craciun fii mai bun si doneaza obiecte casnice pentru a umple casa goala a familiei Andriciuc." +
                            " Casa are o singura camera in care traiesc 5 persoane.",
                Location = location3,
                Tags = new List<Tag>
                {
                    tag5
                },
                ContactInfo = ctInfo2,
                UserId = userId
            });

            ctx.SaveChanges();
            base.Seed(ctx);
        }
    }
}