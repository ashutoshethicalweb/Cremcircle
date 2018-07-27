namespace Cremcircle.Migrations
{
    using Cremcircle.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Cremcircle.Models.DBAuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "University.Models.DBAuthContext";
        }

        protected override void Seed(Cremcircle.Models.DBAuthContext context)
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
            if (context.AppConfigurations.Where(af => af.AppKey == "seedInitComplete" && af.AppConfigValue == "1").Count() == 0)
            {
                //Add Application Configurations
                context.AppConfigurations.AddOrUpdate(af => af.AppKey, new AppConfiguration { Title = "Seed Init Complete", AppKey = "seedInitComplete", AppConfigValue = "1", AppGroup = "App Configuration", FieldType = 0 });
                context.SaveChanges();




                //Roles
                var rols = new List<Role> {
                    new Role { RoleName = "SuperAdmin", IsActive = true },
                    new Role { RoleName = "Admin", IsActive = true },
                    new Role { RoleName = "Full-time Student", IsActive = true },
                    new Role { RoleName = "Part-time Student", IsActive = true },
                    new Role { RoleName = "Concerned Parent of a student", IsActive = true },
                    new Role { RoleName = "High school faculty staff", IsActive = true },

                };
                rols.ForEach(s => context.Roles.AddOrUpdate(p => p.RoleName, s));
                context.SaveChanges();


                //Roles
                var UserAgeDescriptions = new List<UserAgeDescription> {
                    new UserAgeDescription { AgeDescription = "Below 12 years", IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new UserAgeDescription { AgeDescription = "12 to below 18 years", IsActive = true ,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new UserAgeDescription { AgeDescription = "18 to below 24 years", IsActive = true  ,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now},
                    new UserAgeDescription { AgeDescription = "24 to below 32 years", IsActive = true  ,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now},
                    new UserAgeDescription { AgeDescription = "32 to below 45 years", IsActive = true  ,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now},
                    new UserAgeDescription { AgeDescription = "Above 45 years", IsActive = true  ,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now},
                };
                UserAgeDescriptions.ForEach(s => context.UserAgeDescriptions.AddOrUpdate(p => p.AgeDescription, s));
                context.SaveChanges();


                //SecurityTemplates
                var secTemps = new List<SecurityTemplate> {
                    new SecurityTemplate { SecurityTemplateName = "SuperAdmin", IsActive = true },
                    new SecurityTemplate { SecurityTemplateName = "Admin", IsActive = true },
                    new SecurityTemplate { SecurityTemplateName = "Full-time Student", IsActive = true },
                    new SecurityTemplate { SecurityTemplateName = "Part-time Student", IsActive = true },
                    new SecurityTemplate { SecurityTemplateName = "Concerned Parent of a student", IsActive = true },
                    new SecurityTemplate { SecurityTemplateName = "High school faculty staff", IsActive = true },

                };
                secTemps.ForEach(s => context.SecurityTemplates.AddOrUpdate(p => p.SecurityTemplateName, s));
                context.SaveChanges();



                //SecurityTemplates
                var Countrys = new List<Country> {

                    new Country{Name="Afghanistan" ,IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Albania",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Algeria",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Argentina",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Armenia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Australia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Austria",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Azerbaijan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bahrain",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bangladesh",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Belarus",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Belgium",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Belize",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bolivarian Republic of Venezuela",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bolivia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bosnia and Herzegovina",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Brazil",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Brunei Darussalam",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Bulgaria",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Cambodia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Canada",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Caribbean",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Chile",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Colombia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Costa Rica",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Croatia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Czech Republic",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Denmark",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Dominican Republic",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Ecuador",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Egypt",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="El Salvador",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Estonia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Ethiopia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Faroe Islands",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Finland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="France",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Georgia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Germany",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Greece",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Greenland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Guatemala",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Honduras",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Hong Kong S.A.R.",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Hungary",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Iceland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="India",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Indonesia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Iran",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Iraq",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Ireland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Islamic Republic of Pakistan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Israel",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Italy",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Jamaica",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Japan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Jordan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Kazakhstan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Kenya",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Korea",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Kuwait",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Kyrgyzstan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Lao P.D.R.",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Latvia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Lebanon",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Libya",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Liechtenstein",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Lithuania",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Luxembourg",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Macao S.A.R.",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Macedonia (FYROM)",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Malaysia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Maldives",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Malta",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Mexico",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Mongolia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Montenegro",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Morocco",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Nepal",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Netherlands",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="New Zealand",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Nicaragua",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Nigeria",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Norway",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Oman",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Panama",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Paraguay",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="People's Republic of China",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Peru",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Philippines",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Poland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Portugal",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Principality of Monaco",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Puerto Rico",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Qatar",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Republic of the Philippines",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Romania",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Russia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Rwanda",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Saudi Arabia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Senegal",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Serbia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Serbia and Montenegro (Former)",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Singapore",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Slovakia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Slovenia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="South Africa",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Spain",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Sri Lanka",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Sweden",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Switzerland",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Syria",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Taiwan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Tajikistan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Thailand",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Trinidad and Tobago",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Tunisia",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Turkey",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Turkmenistan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="U.A.E.",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Ukraine",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="United Kingdom",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="United States",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Uruguay",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Uzbekistan",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Vietnam",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Yemen",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },
                    new Country{Name="Zimbabwe",IsActive = true,CreatedDate=DateTime.Now,ModifiedDate=DateTime.Now },

                };
                Countrys.ForEach(s => context.Countries.AddOrUpdate(p => p.Name, s));
                context.SaveChanges();




                //Users
                var usrs = new List<User> {
                    new User { LoginName = "Superadmin@gmail.com", Password = "SuperAdmin@123", FirstName = "Super", LastName = "Admin", EmailAddress = "Superadmin@gmail.com",Phonenumber="8800969830", IsActive = true, CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now, RoleID = 1, SecurityTemplateID = 1,UserAgeDescriptionID=1,CountryID=1 }
                };
                usrs.ForEach(s => context.Users.AddOrUpdate(p => p.LoginName, s));
                context.SaveChanges();
            }
        }
    }
}
