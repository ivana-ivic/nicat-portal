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
            AutomaticMigrationDataLossAllowed = true;
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


            //var teacher1 = context.Teachers.FirstOrDefault(t => t.Email.Equals("pera.peric@portal.com"));
            //var course2 = context.Courses.FirstOrDefault(c => c.Id == 2);
            //var course3 = context.Courses.FirstOrDefault(c => c.Id == 3);
            //var course5 = context.Courses.FirstOrDefault(c => c.Id == 5);
            //teacher1.Courses.Add(course2);
            //teacher1.Courses.Add(course3);
            //teacher1.Courses.Add(course5);

            //var course1 = context.Courses.FirstOrDefault(c => c.Id == 1);
            //var course4 = context.Courses.FirstOrDefault(c => c.Id == 4);
            //var teacher2 = context.Teachers.FirstOrDefault(t => t.Email.Equals("nada@portal.com"));
            //teacher2.Courses.Add(course1);
            //teacher2.Courses.Add(course4);

            //var student1 = context.Students.FirstOrDefault(t => t.Email.Equals("itjaiw @gmail.com"));
            //student1.Courses.Add(course1);
            //student1.Courses.Add(course2);
            //student1.Courses.Add(course3);
            //student1.Courses.Add(course4);
            //student1.Courses.Add(course5);

            //var student2 = context.Students.FirstOrDefault(t => t.Email.Equals("mika.mikic@portal.com"));
            //student2.Courses.Add(course1);
            //student2.Courses.Add(course2);
            //student2.Courses.Add(course3);

            //context.SaveChanges();

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
