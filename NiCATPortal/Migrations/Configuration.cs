using NiCATPortal.Models;

namespace NiCATPortal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;
    internal sealed class Configuration : DbMigrationsConfiguration<NiCATPortal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NiCATPortal.Models.ApplicationDbContext context)
        {
            ////create roles
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            //if (!roleManager.RoleExists(Role.ADMIN))
            //    roleManager.Create(new IdentityRole(Role.ADMIN));

            //if (!roleManager.RoleExists(Role.STUDENT))
            //    roleManager.Create(new IdentityRole(Role.STUDENT));

            //if (!roleManager.RoleExists(Role.TEACHER))
            //    roleManager.Create(new IdentityRole(Role.TEACHER));

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            //create administrator
            var adminEmail = WebConfigurationManager.AppSettings["AdministratorEmail"];
            if (!context.Administrators.Any(x => x.UserName == adminEmail))
            {
                var password = new ApplicationUserManager(new UserStore<ApplicationUser>(context)).PasswordHasher.HashPassword("adminpass");
                var administrator = new Administrator()
                {
                    //AdministratorProperty = "Called from seed",
                    Email = adminEmail,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    UserName = adminEmail,
                    PasswordHash = password,
                    Name = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "060/1234567"
                };

                context.Administrators.AddOrUpdate(administrator);
                context.SaveChanges();

                var adminManager = new UserManager<Administrator>(new UserStore<Administrator>(context));
                adminManager.AddToRole(administrator.Id, Role.ADMIN);
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //var passwordHash = new PasswordHasher();
            //string password = passwordHash.HashPassword("adminpass");
            //var admin = new Administrator { Name = "Admin", LastName = "Admin", Email = "admin@user.com", UserName = "admin@user.com", PhoneNumber = "060/1234567", PasswordHash = password };
            //context.Users.AddOrUpdate(u => u.UserName,admin);
        }
    }
}
