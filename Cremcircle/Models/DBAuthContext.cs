using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Cremcircle.Models
{
    public class DBAuthContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DBAuthContext() : base("name=DBAuthContext")
        {
            //Database.SetInitializer<DBAuthContext>(new CreateDatabaseIfNotExists<DBAuthContext>());
            //Database.SetInitializer<DBAuthContext>(new DropCreateDatabaseIfModelChanges<DBAuthContext>());
            //Database.SetInitializer<DBAuthContext>(new DropCreateDatabaseAlways<DBAuthContext>());

           Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBAuthContext, Cremcircle.Migrations.Configuration>("DBAuthContext"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<Cremcircle.Models.AppConfiguration> AppConfigurations { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.Role> Roles { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.SecurityTemplate> SecurityTemplates { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.Permission> Permissions { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.SecurityTemplatePermission> SecurityTemplatePermissions { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.UserLog> UserLogs { get; set; }

       public System.Data.Entity.DbSet<Cremcircle.Models.Country> Countries { get; set; }

        public System.Data.Entity.DbSet<Cremcircle.Models.UserAgeDescription> UserAgeDescriptions { get; set; }



    }
}
