namespace LandmarksBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using LandmarksBlog.Models;

    public sealed class DbMigrationConfig : DbMigrationsConfiguration<LandmarksBlog.Models.ApplicationDbContext>
    {
        public DbMigrationConfig()
        {
            AutomaticMigrationsEnabled = true;
            //ContextKey = "LandmarksBlog.Models.ApplicationDbContext";
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
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

            if (!context.Users.Any())
            {
                var adminEmail = "neli@neli.com";
                var adminUserName = "neli";
                var adminFullName = "Neli Neli";
                var adminPassword = "123";
                string adminRole = "Administrator";
                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateSeveralEvents(context);
            }
        }

        private void CreateSeveralEvents(ApplicationDbContext context)
        {
            context.Posts.Add(new Post()
            {
                Title = "Забележителности",
                Date = DateTime.Now.Date,
                Author = context.Users.First(),
            });

            context.Posts.Add(new Post()
            {
                Title = "От света <Anonymous>",
                Date = DateTime.Now.Date.AddDays(-2),
                Comments = new HashSet<Comment>()
                {
                    new Comment()
                    {
                        Text = "<Anonymous> comment"
                    },
                    new Comment()
                    {
                        Text = "User comment", Author = context.Users.First()
                    }
                }
            });
        }

        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            //Create the "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create the "Administrattor" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
            //Add the "admin" user to "Administrator" role

            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

    }
}
