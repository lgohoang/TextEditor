namespace TextEditor.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TextEditor.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TextEditor.Models.ApplicationDbContext context)
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

            context.PageFormat.AddOrUpdate(
                    p => p.Id,
                    new Models.PageFormat { Id = 1, Name = "Cover", PaperType = "A4" },
                    new Models.PageFormat { Id = 2, Name = "Side Cover", PaperType = "A4" },
                    new Models.PageFormat { Id = 3, Name = "Normal", PaperType = "A4" }

                );
        }
    }
}
